using evicalc.models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

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
            const double digitsNull = 0;
            const double minimumDigits = 2;
            double numberDigits = request.Addens.Count();


            //return BadRequest("Hello");

            //if (request.Addens == null)
            if (numberDigits == digitsNull)
            {
                return BadRequest("Addens cannot be null");
            }
            else if (numberDigits < minimumDigits)
            {
                return BadRequest("Addens param requires at least two numbers to do something");
            }
            /*
            else if(request.Addens.GetType() == typeof(string))
            {
                return BadRequest("Addens must be a number, not a text");
            }
            */

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
        //public SubResponse Sub(SubRequest request)
        public IActionResult Sub(SubRequest request)
        {
            /*
            if (request.Minuend == null) // Try this
            {
                return BadRequest("Minuend cannot be null");
            }
            */
            //return new SubResponse() { Difference = request.Minuend - request.Subtrahend };
            return Ok(new SubResponse() { Difference = request.Minuend - request.Subtrahend });
        }

        [HttpPost]
        public IActionResult Mult(MultRequest request)
        {
            const double digitsNull = 0;
            const double minimumDigits = 2;

            double numberDigits = request.Factors.Count();
            double[] arrayIenumerable = request.Factors.ToArray();
            double result = 1; // Because if this is 0 or not exist, the result always will be 0 after the operation


            if (numberDigits == digitsNull)
            {
                return BadRequest("Factors cannot be null");
            }
            else if (numberDigits < minimumDigits)
            {
                return BadRequest("Factors param requires at least two numbers to do something");
            }

            foreach (var item in arrayIenumerable)
            {
                result *= item;
            }

            return Ok(new MultResponse() { Product = result });
        }

        [HttpPost]
        public IActionResult Div(DivRequest request)
        {
            const double illegalDivisor = 0;

            if(request.Divisor == illegalDivisor)
            {
                return BadRequest("The divider can't be 0");
            }

            return Ok(new DivResponse() { Quotient = request.Dividend / request.Divisor, Remainder = request.Dividend % request.Divisor });
        }

        [HttpPost]
        public IActionResult Sqrt(SqrtRequest request)
        {
            return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
        }
    }
}
