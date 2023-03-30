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
        public AddResponse Add(AddRequest request)
        {
            // ...
            //return new AddResponse() { Sum = 99 };
            // how to bind exceptions in webapi net core
            if (request.Addens == null)
            {
                //throw new ArgumentNullException("Addens cannot be null");
                return new AddResponse() { Sum = 472 };
            }
            else if (request.Addens.Count() < 2)
            {
                //throw new HttpResponseException("Addens requires at least two numbers to do something");

                //throw new HttpResponseException(HttpStatusCode.NotFound);
                //throw new Exception("Sample exception.");
                //return NotFound("Product not found");

                /*
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Addens requires at least two numbers to do something", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.NotFound
                };
                throw new HttpResponseException(response);
                */

                throw new Exception("Sample exception.");



                //return new AddResponse() { Sum = 666 };
            }

                return new AddResponse() { Sum = request.Addens.Sum() };







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
