using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Dtos.Evaluation;
using Bss.Api.Dtos.Input;
using Bss.Api.Mappers;
using Bss.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bss.Api.Controllers
{
    [Route("api/evaluation")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;
        private readonly IBeSportSmartDbContext _dbContext;

        public EvaluationController(IEvaluationService evaluationService, IBeSportSmartDbContext dbContext)
        {
            _evaluationService = evaluationService;
            _dbContext = dbContext;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _evaluationService.GetAllSports());
        }

        [HttpGet("getAllWithCovering")]
        public async Task<IActionResult> GetAllWithCovering()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _evaluationService.GetAllSportsWithCovering());
        }

        [HttpGet("getInputs/{sport}")]
        public async Task<IActionResult> GetInputs([FromRoute] string sport)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _evaluationService.GetRequiredInputs(sport));
        }

        [HttpPost("evaluate")]
        public async Task<IActionResult> Evaluate([FromBody] EvaluationRequestDto evaluationRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _evaluationService.Evaluate(evaluationRequest));
        }

        //private readonly InputDto[] GetInputs(string name)
        //{
        //    var inputs = await _dbContext.Inputs
        //        .Where(x => x.ScoreProviderInputs.Any(spi => spi.ScoreProvider.Name == name))
        //        .Select(x => x.ToInputDto())
        //        .ToArray();

        //    return inputs;
        //}
    }
}