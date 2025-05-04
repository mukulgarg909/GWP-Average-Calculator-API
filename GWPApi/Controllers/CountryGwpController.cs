using GWPApi.Models;
using GWPApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GWPApi.Controllers
{
    [ApiController]
    [Route("server/api/gwp")]
    public class CountryGwpController : ControllerBase
    {
        private readonly IGwpCalculatorService _calculatorService;

        public CountryGwpController(IGwpCalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost("avg")]
        [ProducesResponseType(typeof(GwpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateAverage([FromBody] GwpRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request payload is required");
            }

            if (string.IsNullOrEmpty(request.Country) || request.Country.Length != 2)
            {
                return BadRequest("Country code must be provided and must be 2 characters");
            }

            if (request.Lob == null || !request.Lob.Any() || request.Lob.Any(lob => string.IsNullOrWhiteSpace(lob))) 
            {
                return BadRequest("At least one line of business is required");
            }

            var result = await _calculatorService.CalculateAverage(request);
            return Ok(result);
       
        }
    }
}
