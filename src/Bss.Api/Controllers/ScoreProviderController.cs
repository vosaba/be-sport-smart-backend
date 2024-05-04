
using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Dtos.ScoreProvider;
using Bss.Api.Mappers;
using Bss.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Controllers
{
    [Route("api/scoreProvider")]
    [ApiController]
    public class ScoreProviderController : ControllerBase
    {
        private readonly IBeSportSmartDbContext _dbContext;
        private readonly IInputRepository _inputRepository;
        private readonly IScoreProviderRepository _scoreProviderRepository;
        private readonly IFormulaService _calculationEngine;

        public ScoreProviderController(IBeSportSmartDbContext dbContext, IScoreProviderRepository scoreProviderRepository, IInputRepository inputRepository, IFormulaService calculationEngine)
        {
            _dbContext = dbContext;
            _inputRepository = inputRepository;
            _scoreProviderRepository = scoreProviderRepository;
            _calculationEngine = calculationEngine;
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ScoreProviderDto scoreProvider)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _calculationEngine.Validate(scoreProvider.Formula);

            if (!isValid.isValid)
                return BadRequest(isValid.error);

            var (measureNames, inputNames) = _calculationEngine.GetDependents(scoreProvider.Formula);

            var newScoreProvider = new ScoreProvider
            {
                Name = scoreProvider.Name,
                Formula = scoreProvider.Formula,
                DependentProviders = measureNames
            };

             _scoreProviderRepository.AddScoreProvider(newScoreProvider, await _inputRepository.GetInputs(inputNames));

            await _scoreProviderRepository.ApplyChanges();

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

            var dependentProviders = await _dbContext.ScoreProviders
                .Where(x => x.DependentProviders.Contains(name))
                .Select(x => x.Name)
                .ToListAsync();

            if (dependentProviders.Any())
                return BadRequest($"Score provider is used by some other score providers: {string.Join(", ", dependentProviders)}");

            _scoreProviderRepository.Remove(scoreProvider);

            await _scoreProviderRepository.ApplyChanges();

            return Ok();
        }
    }
}