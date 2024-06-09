using Bss.Api.Data;
using Bss.Api.Data.Models;
using Bss.Api.Dtos.Input;
using Bss.Api.Mappers;
using Bss.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bss.Api.Controllers
{
    [Route("api/input")]
    [Authorize(Roles = "Trainer,Admin")]
    [ApiController]
    public class InputController : ControllerBase
    {
        private readonly IBeSportSmartDbContext _dbContext;

        private readonly IEvaluationService _evaluationService;

        public InputController(IBeSportSmartDbContext dbContext, IEvaluationService evaluationService)
        {
            _dbContext = dbContext;
            _evaluationService = evaluationService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var input = await _dbContext.Inputs.SingleOrDefaultAsync(x => x.Name == name);

            if (input == null)
                return NotFound();

            return Ok(input.ToInputDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] InputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInput = await _dbContext.Inputs.SingleOrDefaultAsync(x => x.Name == input.Name);

            if (existingInput != null)
                return BadRequest("Input already exists");

            _dbContext.Add(new Input
            {
                Name = input.Name,
                Type = input.Type,
                Options = input.Options,
            });

            await _dbContext.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var input = await _dbContext.Inputs.SingleOrDefaultAsync(x => x.Name == name);

            if (input == null)
                return NotFound();

            if (await _dbContext.ScoreProviders.AnyAsync(x => x.DependentInputs.Contains(name)))
                return BadRequest("Input is used by some score providers");

            _dbContext.Remove(input);

            await _dbContext.ApplyChanges();

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] InputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInput = await _dbContext.Inputs.SingleOrDefaultAsync(x => x.Name == input.Name);

            if (existingInput == null)
                return NotFound();

            existingInput.Type = input.Type;
            existingInput.Options = input.Options;

            await _dbContext.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }
    }
}