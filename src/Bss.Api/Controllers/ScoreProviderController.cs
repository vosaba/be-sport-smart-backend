
using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Dtos.ScoreProvider;
using Bss.Api.Extensions;
using Bss.Api.Mappers;
using Bss.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bss.Api.Controllers
{
    [Route("api/scoreProvider")]
    [Authorize(Roles = "Trainer,Admin")]
    [ApiController]
    public class ScoreProviderController : ControllerBase
    {
        private readonly IInputRepository _inputRepository;
        private readonly IScoreProviderRepository _scoreProviderRepository;
        private readonly IFormulaService _formulaService;
        private readonly IEvaluationService _evaluationService;

        public ScoreProviderController(
            IScoreProviderRepository scoreProviderRepository,
            IInputRepository inputRepository,
            IFormulaService calculationEngine,
            IEvaluationService evaluationService)
        {
            _inputRepository = inputRepository;
            _scoreProviderRepository = scoreProviderRepository;
            _formulaService = calculationEngine;
            _evaluationService = evaluationService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var scoreProvider = await _scoreProviderRepository.GetScoreProvider(name);

            if (scoreProvider == null)
                return NotFound();

            return Ok(scoreProvider.ToScoreProviderDto());
        }

        [HttpPost("getFiltered")]
        public async Task<IActionResult> GetAll([FromBody] ScoreProviderFilterDto scoreProviderFilter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var scoreProviders = await _scoreProviderRepository.GetScoreProviders(scoreProviderFilter);

            return Ok(scoreProviders.Select(x => x.ToScoreProviderListItemDto()));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ScoreProviderDto scoreProvider)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _formulaService.Validate(scoreProvider.Formula);

            if (!isValid.isValid)
                return BadRequest(isValid.error);

            var (measureNames, scoreNames, inputNames) = _formulaService.GetDependents(scoreProvider.Formula);

            var date = DateTime.UtcNow;
            var newScoreProvider = new ScoreProvider
            {
                Name = scoreProvider.Name,
                Type = scoreProvider.Type,
                Formula = scoreProvider.Formula,
                DependentProviders = measureNames.Concat(scoreNames).ToArray(),
                Created = date,
                Updated = date,
                CreatedBy = User.GetUsername(),
                Disabled = scoreProvider.Disabled,
            };

             _scoreProviderRepository.AddScoreProvider(newScoreProvider, await _inputRepository.GetInputs(inputNames));

            await _scoreProviderRepository.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ScoreProviderDto scoreProvider)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingScoreProvider = await _scoreProviderRepository.GetScoreProvider(scoreProvider.Name);

            if (existingScoreProvider == null)
                return NotFound();

            var isValid = await _formulaService.Validate(scoreProvider.Formula);

            if (!isValid.isValid)
                return BadRequest(isValid.error);

            var (measureNames, scoreNames, inputNames) = _formulaService.GetDependents(scoreProvider.Formula);

            existingScoreProvider.Formula = scoreProvider.Formula;
            existingScoreProvider.Type = scoreProvider.Type;
            existingScoreProvider.DependentProviders = measureNames.Concat(scoreNames).ToArray();

            _scoreProviderRepository.UpdateScoreProvider(existingScoreProvider, await _inputRepository.GetInputs(inputNames));

            await _scoreProviderRepository.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var scoreProvider = await _scoreProviderRepository.GetScoreProvider(name);

            if (scoreProvider == null)
                return NotFound();

            var dependentProviders = await _scoreProviderRepository.GetDependenstOfScoreProvider(name);

            if (dependentProviders.Any())
                return BadRequest($"Score provider is used by some other score providers: {string.Join(", ", dependentProviders)}");

            _scoreProviderRepository.Remove(scoreProvider);

            await _scoreProviderRepository.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }
    }
}