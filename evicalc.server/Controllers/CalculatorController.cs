using evicalc.models;
using Microsoft.AspNetCore.Mvc;

namespace evicalc.server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Add(AddRequest request)
        {
            if (request.Addens == null)
            {
                return BadRequest("Addens cannot be null");
            }
            else if (request.Addens.Count() < 2)
            {
                return BadRequest("Addens param requires at least two numbers to do something");
            }

            return Ok(new AddResponse() { Sum = request.Addens.Sum() });



            /*
             * Variations that I think...
             *
             * Empty
             * 1 Number (Maybe an error or the second number could be 0 in add)
             * Words and simbols (Error)
             * Code Injection (Error)
             * Decimals with "," or "."  (I think "," is good but "." is ignored, so try to parse to string, change the "." for "," and parse to double again)
             * Decimals with many "," and/or "." (Maybe an error or parse to string and take only the first one an remove the other ones, and then parse to double again)
            */
        }

        [HttpPost]
        public SubResponse Sub(SubRequest request)
        {
            return new SubResponse() { Difference = request.Minuend - request.Subtrahend };
        }

    }
}
