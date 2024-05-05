using Bss.Api.Data.Models;
using Bss.Api.Data.Repositories;
using Bss.Api.Dtos.Input;
using Bss.Api.Mappers;
using Bss.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bss.Api.Controllers
{
    [Route("api/input")]
    [Authorize(Roles = "Trainer,Admin")]
    [ApiController]
    public class InputController : ControllerBase
    {
        private readonly IInputRepository _inputRepository;
        private readonly IEvaluationService _evaluationService;

        public InputController(IInputRepository inputRepository, IEvaluationService evaluationService)
        {
            _inputRepository = inputRepository;
            _evaluationService = evaluationService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var input = await _inputRepository.GetInput(name);

            if (input == null)
                return NotFound();

            return Ok(input.ToInputDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] InputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInput = await _inputRepository.GetInput(input.Name);

            if (existingInput != null)
                return BadRequest("Input already exists");

            _inputRepository.Add(new Input
            {
                Name = input.Name,
                Type = input.Type,
                Options = input.Options,
            });

            await _inputRepository.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var input = await _inputRepository.GetInput(name);

            if (input == null)
                return NotFound();

            if (input.ScoreProviderInputs.Any())
                return BadRequest("Input is used by some score providers");

            _inputRepository.Remove(input);

            await _inputRepository.ApplyChanges();

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] InputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInput = await _inputRepository.GetInput(input.Name);

            if (existingInput == null)
                return NotFound();

            existingInput.Type = input.Type;
            existingInput.Options = input.Options;

            await _inputRepository.ApplyChanges();

            await _evaluationService.RefreshContext();

            return Ok();
        }
    }
}