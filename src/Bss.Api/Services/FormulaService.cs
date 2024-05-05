using Bss.Api.Data;
using Bss.Api.Data.Models;
using Jint;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace Bss.Api.Services
{
    public interface IFormulaService : IDisposable
    {
        public Task<(bool isValid, string? error)> Validate(string formula);
        public (string[] measures, string[] scores, string[] inputs) GetDependents(string formula);
        public string[] GetAllInputs(string formula);
    }

    public class FormulaService : IFormulaService
    {
        private readonly Regex _measuresRegex = new Regex(@"measures\.(\w+)");
        private readonly Regex _scoresRegex = new Regex(@"scores\.(\w+)");
        private readonly Regex _inputsRegex = new Regex(@"inputs\.(\w+)");

        private readonly Engine _testEngine = new Engine();
        private readonly IBeSportSmartDbContext _dbContext;

        public FormulaService(IBeSportSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool isValid, string? error)> Validate(string formula)
        {
            var (measures, scores, inputs) = GetDependents(formula);

            var existingMeasures =  await _dbContext.ScoreProviders
                .Where(sp => measures.Contains(sp.Name) && sp.Type == ScoreProviderType.Measure && !sp.Disabled)
                .Select(sp => sp.Name)
                .ToListAsync();

            var existingScores = await _dbContext.ScoreProviders
                .Where(sp => scores.Contains(sp.Name) && sp.Type == ScoreProviderType.Score && !sp.Disabled)
                .Select(sp => sp.Name)
                .ToListAsync();

            var existingInputs = await _dbContext.Inputs
                .Where(i => inputs.Contains(i.Name))
                .Select(i => i.Name)
                .ToListAsync();

            var missingMeasures = measures.Except(existingMeasures);
            var missingScores = scores.Except(existingScores);
            var missingInputs = inputs.Except(existingInputs);

            if (missingMeasures.Any() || missingScores.Any() || missingInputs.Any())
            {
                var errorMessageBuilder = new StringBuilder();

                if (missingMeasures.Any())
                {
                    errorMessageBuilder.Append($"Missing measures: {string.Join(", ", missingMeasures)}");
                }

                if (missingScores.Any())
                {
                    if (errorMessageBuilder.Length > 0)
                    {
                        errorMessageBuilder.Append("; ");
                    }

                    errorMessageBuilder.Append($"Missing scores: {string.Join(", ", missingScores)}");
                }

                if (missingInputs.Any())
                {
                    if (errorMessageBuilder.Length > 0)
                    {
                        errorMessageBuilder.Append("; ");
                    }

                    errorMessageBuilder.Append($"Missing inputs: {string.Join(", ", missingInputs)}");
                }

                return (false, errorMessageBuilder.ToString());
            }

            try
            {
                _testEngine.Execute($"var test = {formula}");

                if (_testEngine.Evaluate("typeof test === 'function'").AsBoolean() != true)
                {
                    return (false, "Formula must be a function");
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

            return (true, null);
        }

        public (string[] measures, string[] scores, string[] inputs) GetDependents(string formula)
        {
            var measures = _measuresRegex
                .Matches(formula)
                .Select(m => m.Groups[1].Value)
                .Distinct();

            var scores = _scoresRegex
                .Matches(formula)
                .Select(m => m.Groups[1].Value)
                .Distinct();

            var inputs = _inputsRegex
                .Matches(formula)
                .Select(m => m.Groups[1].Value)
                .Distinct();

            return (measures.ToArray(), scores.ToArray(), inputs.ToArray());
        }

        public void Dispose()
        {
            _testEngine.Dispose();
        }

        public string[] GetAllInputs(string formula)
        {
            throw new NotImplementedException();
        }
    }
}
