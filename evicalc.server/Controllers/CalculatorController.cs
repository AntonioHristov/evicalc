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
			const double DIGITSNULL = 0;
			const double MINIMUMDIGITS = 2;
			var numbers = request.Addens;
			var length = numbers.Count();
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

			if (length == DIGITSNULL)
				return BadRequest("Addens cannot be null");
			else if (length < MINIMUMDIGITS)
				return BadRequest("Addens param requires at least two numbers to do something");
			/*
			// Another exception before these if, I think it should be in client instead here
			else if(request.Addens.GetType() == typeof(string))
				return BadRequest("Addens must be a number, not a text");
			*/
			var resultOperation = numbers.Sum();
			QueryResponse.AddDataOperation(QueryResponse.OPERATOR_ADD, numbers, resultOperation);
			return Ok(new AddResponse() { Sum = resultOperation });
		}

		[HttpPost]
		public IActionResult Sub(SubRequest request)
		{
			var numbers = new List<double>() { request.Minuend, request.Subtrahend };
			var resultOperation = request.Minuend - request.Subtrahend;

			QueryResponse.AddDataOperation(QueryResponse.OPERATOR_SUB, numbers, resultOperation);
			return Ok(new SubResponse() { Difference = resultOperation });
		}

		[HttpPost]
		public IActionResult Mult(MultRequest request)
		{
			const double digitsNull = 0;
			const double minimumDigits = 2;
			var numberDigits = request.Factors.Count();

			if (numberDigits == digitsNull)
				return BadRequest("Factors cannot be null");
			else if (numberDigits < minimumDigits)
				return BadRequest("Factors param requires at least two numbers to do something");

			var numbers = request.Factors;
			var result = 1D; // Because if this is 0, not exist or has another value, the result always will be 0 after the operation
			foreach (var item in numbers)
			{
				result *= item;
			}
			QueryResponse.AddDataOperation(QueryResponse.OPERATOR_MUL, numbers, result);
			return Ok(new MultResponse() { Product = result });
		}

		[HttpPost]
		public IActionResult Div(DivRequest request)
		{
			const double illegalDivisor = 0;

			if (request.Divisor == illegalDivisor)
				return BadRequest("The divider can't be 0");

			var numbers = new List<double>() { request.Dividend, request.Divisor };
			var result = request.Dividend / request.Divisor;
			QueryResponse.AddDataOperation(QueryResponse.OPERATOR_DIV, numbers, result);
			return Ok(new DivResponse() { Quotient = result, Remainder = request.Dividend % request.Divisor });
		}

		[HttpPost]
		public IActionResult Sqrt(SqrtRequest request)
		{
			var numbers = new List<double>() { request.Number };
			var result = Math.Sqrt(numbers.First());

			QueryResponse.AddDataOperation(QueryResponse.OPERATOR_SQR, numbers, result);
			return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
		}

		[HttpPost]
		public IActionResult Query(QueryRequest request)
		{
			var result = QueryResponse.GetListDataOperationById(request.Id);

			if (result == null)
			{
				var error = $"The ID {request.Id} doesn't exist. The allowed Ids are: {Environment.NewLine} All Ids: {QueryResponse.ID_ALL_IDS} {Environment.NewLine} {string.Join(Environment.NewLine + Common.BLANK_SPACE, QueryResponse.GetListAllIdsOperations())}";
				return BadRequest(error);
			}
			else if (result.Count == Common.LENGHT_LIST_EMPTY)
			{
				var error = "It's empty because you haven't created any operations yet";
				return BadRequest(error);
			}
			return Ok(result);
		}
	}
}
