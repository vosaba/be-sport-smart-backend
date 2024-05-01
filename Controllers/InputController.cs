using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/input")]
    [ApiController]
    public class InputController : ControllerBase
    {
        private readonly IInputRepository _inputRepository;
        public InputController(IInputRepository inputRepository)
        {
            _inputRepository = inputRepository;
        }

        [HttpGet("{scoreProvider}")]
        public async Task<IActionResult> Get([FromRoute]string scoreProvider)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var inputs = await _inputRepository.GetInputs(scoreProvider);

            //var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(null);
        }

 

    }
}