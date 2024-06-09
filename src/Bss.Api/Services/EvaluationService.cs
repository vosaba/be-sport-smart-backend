using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Dtos.Evaluation;
using Jint;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;

namespace Bss.Api.Services
{
    public interface IEvaluationService
    {
        Task<string[]> GetAllSports();
        Task<IDictionary<string, string[]>> GetAllSportsWithCovering();
        Task<string[]> GetCovering(string scoreProvider);
        Task<string[]> GetRequiredInputs(string scoreProvider);
        Task<(bool isValid, string? error)> ValidateInputs(string scoreProvider, IDictionary<string, string> inputValues);
        Task<EvaluationResultDto> Evaluate(EvaluationRequestDto evaluationRequest);
        Task RefreshContext();
    }

    public class EvaluationService : IEvaluationService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IBeSportSmartDbContext _dbContext;
        private readonly IFormulaService _formulaService;
        private readonly IEvaluationEngine _evaluationEngine;

        public EvaluationService(IMemoryCache memoryCache, IBeSportSmartDbContext dbContext, IFormulaService formulaService, IEvaluationEngine evaluationEngine)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
            _formulaService = formulaService;
            _evaluationEngine = evaluationEngine;
        }

        public async Task<string[]> GetAllSports()
        {
            var cacheKey = "all_sports";

            if (_memoryCache.TryGetValue(cacheKey, out string[]? cachedSportNames))
            {
                return cachedSportNames!;
            }

            return _memoryCache.Set(cacheKey, await _dbContext.ScoreProviders
                .Where(x => x.Type == ScoreProviderType.Sport && !x.Disabled)
                .Select(x => x.Name)
                .ToArrayAsync());
        }

        public async Task<EvaluationResultDto> Evaluate(EvaluationRequestDto evaluationRequest)
        {
            var (isValid, error) = await ValidateInputs(evaluationRequest.Name, evaluationRequest.Inputs);

            if (!isValid)
            {
                throw new InvalidOperationException($"Unable to evaluate, details: {error}");
            }

            if (!_evaluationEngine.Initialized)
            {
                await RefreshContext();
            }

            var cacheKey = $"formula:{evaluationRequest.Name}";

            if (!_memoryCache.TryGetValue(cacheKey, out string? formula))
            {
                formula = _memoryCache.Set(cacheKey, await _dbContext.ScoreProviders
                    .Where(x => x.Name == evaluationRequest.Name)
                    .Select(x => x.Formula)
                    .SingleAsync());
            }

            var formattedInputs = await GetFormatedInputs(evaluationRequest.Inputs);

            var score = await _evaluationEngine.GetScore(formula!, formattedInputs);

            return new EvaluationResultDto
            {
                Name = evaluationRequest.Name,
                Score = score
            };
        }

        public async Task<string[]> GetRequiredInputs(string scoreProvider)
        {
            var cacheKey = $"inputs:{scoreProvider}";

            if (_memoryCache.TryGetValue(cacheKey, out string[]? cachedInputNames))
            {
                return cachedInputNames!;
            }

            var inputNames = new HashSet<string>();
            var visitedMeasures = new HashSet<string>(); // To avoid revisiting measures

            async Task ProcessMeasure(string measureName)
            {
                if (visitedMeasures.Contains(measureName))
                    return;

                visitedMeasures.Add(measureName);

                var measure = await _dbContext.ScoreProviders.SingleOrDefaultAsync(x => x.Name == measureName && !x.Disabled);

                if (measure == null)
                    return;

                var (measures, scoreNames, inputs) = _formulaService.GetDependents(measure.Formula);

                foreach (var input in inputs)
                {
                    inputNames.Add(input);
                }

                var dependentProviders = measures.Concat(scoreNames);

                foreach (var dependentProvider in dependentProviders)
                {
                    await ProcessMeasure(dependentProvider); // Recursively process dependent measures
                }
            }

            await ProcessMeasure(scoreProvider);

            return _memoryCache.Set(cacheKey, inputNames.ToArray());
        }

        public async Task RefreshContext()
        {
            var measures = await _dbContext.ScoreProviders.Where(x => x.Type == ScoreProviderType.Measure).ToArrayAsync();
            var scores = await _dbContext.ScoreProviders.Where(x => x.Type == ScoreProviderType.Score).ToArrayAsync();

            _evaluationEngine.RefreshContext(measures, scores);

            CLearCache();
        }

        public async Task<(bool isValid, string? error)> ValidateInputs(string scoreProvider, IDictionary<string, string> inputValues)
        {
            var requiredInputs = await GetRequiredInputs(scoreProvider);

            var missingInputs = requiredInputs.Except(inputValues.Keys);
            if (missingInputs.Any())
            {
                return (false, $"Some inputs are missing: {string.Join(",", missingInputs)}");
            }

            var inputs = await GetInputs(inputValues.Keys);

            foreach (var input in inputs)
            {
                switch (input.Type)
                {
                    case InputType.Number:
                        if (!double.TryParse(inputValues[input.Name], out _))
                        {
                            return (false, $"Invalid type for input {input.Name}");
                        }
                        break;
                    case InputType.String:
                        break;
                    case InputType.Boolean:
                        if (!bool.TryParse(inputValues[input.Name], out _))
                        {
                            return (false, $"Invalid type for input {input.Name}");
                        }
                        break;
                    default:
                        return (false, $"Unknown input type: {input.Type}");
                }

                if (input.Options.Any() && !input.Options.Contains(inputValues[input.Name]))
                {
                    return (false, $"Invalid value for input {input.Name}, possible values are : {string.Join(", ", input.Options)}");
                }
            }

            return (true, null);
        }

        private async Task<IDictionary<string, string>> GetFormatedInputs(IDictionary<string, string> inputValues)
        {
            var formattedValues = new Dictionary<string, string>();

            var inputs = await GetInputs(inputValues.Keys);

            foreach (var input in inputs)
            {
                switch (input.Type)
                {
                    case InputType.String:
                        formattedValues[input.Name] = $"'{inputValues[input.Name]}'";
                        break;
                    default:
                        formattedValues[input.Name] = inputValues[input.Name];
                        break;
                }
            }

            return formattedValues;
        }

        private async Task<IEnumerable<Input>> GetInputs(IEnumerable<string> names)
        {
            string cacheKey = $"all_inputs";
            IEnumerable<Input> allInputs;

            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Input>? cachedInputs))
            {
                allInputs = cachedInputs!;
            }
            else
            {
                allInputs = _memoryCache.Set(cacheKey, await _dbContext.Inputs.ToListAsync());
            }

            return allInputs
                    .Where(x => names.Contains(x.Name))
                    .ToList();
        }

        private void CLearCache()
        {
            if (_memoryCache is MemoryCache concreteMemoryCache)
            {
                concreteMemoryCache.Clear();
            }
        }

        public async Task<IDictionary<string, string[]>> GetAllSportsWithCovering()
        {
            var cacheKey = "all_sports_with_covering";

            if (_memoryCache.TryGetValue(cacheKey, out IDictionary<string, string[]>? cachedSportNamesWithCovering))
            {
                return cachedSportNamesWithCovering!;
            }

            var result = new Dictionary<string, string[]>();

            foreach (var sport in await GetAllSports())
            {
                result[sport] = await GetCovering(sport);
            }

            return _memoryCache.Set(cacheKey, result);
        }

        public async Task<string[]> GetCovering(string scoreProvider)
        {
            var cacheKey = $"covering:{scoreProvider}";

            if (_memoryCache.TryGetValue(cacheKey, out string[]? cachedInputNames))
            {
                return cachedInputNames!;
            }

            var inputNames = new HashSet<string>();
            var visitedMeasures = new HashSet<string>(); // To avoid revisiting measures

            async Task ProcessMeasure(string measureName)
            {
                if (visitedMeasures.Contains(measureName))
                    return;

                visitedMeasures.Add(measureName);

                var measure = await _dbContext.ScoreProviders.SingleOrDefaultAsync(x => x.Name == measureName && !x.Disabled);

                if (measure == null)
                    return;

                var (measures, scoreNames, inputs) = _formulaService.GetDependents(measure.Formula);

                foreach (var input in inputs)
                {
                    inputNames.Add(input);
                }

                var dependentProviders = measures.Concat(scoreNames);

                foreach (var dependentProvider in dependentProviders)
                {
                    await ProcessMeasure(dependentProvider); // Recursively process dependent measures
                }
            }

            await ProcessMeasure(scoreProvider);

            visitedMeasures.Remove(scoreProvider);

            return _memoryCache.Set(cacheKey, visitedMeasures.ToArray());
        }
    }
}
