using evicalc.models;
using Microsoft.AspNetCore.Mvc;

namespace evicalc.server.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class CalculatorController : ControllerBase
	{
		#region Fields

		private const string REQUEST_CANNOT_BE_NULL = "Invalid request received, look like cannot be deserialized properly";

		private readonly ILogger<CalculatorController> _logger;

		#endregion

		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Add(AddRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			const double MINIMUMDIGITS = 2;

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

			if (request.Addens?.Any() != true) //< Null or empty..
			{
				return BadRequest("Addens cannot be null or empty");
			}
			else if (request.Addens.Count < MINIMUMDIGITS)
			{
				return BadRequest("Addens param requires at least two numbers to do something");
			}
			/*
			// Another exception before these if, I think it should be in client instead here
			else if(request.Addens.GetType() == typeof(string))
				return BadRequest("Addens must be a number, not a text");
			*/
			var result = request.Addens.Sum();
			var calculation = $"{string.Join(" + ", request.Addens)} = {result}"; //< ie. "N + Y = X"
			Journal.AddDataOperation(Operation.OPERATOR_ADD, calculation, result);
			return Ok(new AddResponse() { Sum = result });
		}

		[HttpPost]
		public IActionResult Sub(SubRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			var numbers = new List<double>() { request.Minuend, request.Subtrahend };
			var result = request.Minuend - request.Subtrahend;
			var calculation = $"{string.Join(" - ", numbers)} = {result}";
			Journal.AddDataOperation(Operation.OPERATOR_SUB, calculation, result);
			return Ok(new SubResponse() { Difference = result });
		}

		[HttpPost]
		public IActionResult Mult(MultRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			const double digitsNull = 0;
			const double minimumDigits = 2;
			var numberDigits = request.Factors.Count();

			if (numberDigits == digitsNull)
				return BadRequest("Factors cannot be null");
			else if (numberDigits < minimumDigits)
				return BadRequest("Factors param requires at least two numbers to do something");

			var result = 1D; // Because if this is 0, not exist or has another value, the result always will be 0 after the operation
			foreach (var item in request.Factors)
			{
				result *= item;
			}
			var calculation = $"{string.Join(" * ", request.Factors)} = {result}";
			Journal.AddDataOperation(Operation.OPERATOR_MUL, calculation, result);
			return Ok(new MultResponse() { Product = result });
		}

		[HttpPost]
		public IActionResult Div(DivRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			const double illegalDivisor = 0;

			if (request.Divisor == illegalDivisor)
				return BadRequest("The divider can't be 0");

			var numbers = new List<double>() { request.Dividend, request.Divisor };
			var result1 = request.Dividend / request.Divisor;
			var result2 = request.Dividend % request.Divisor;
			var calculation = $"{string.Join(" / ", numbers)} = {result1} | {string.Join(" % ", numbers)} = {result2}";
			Journal.AddDataOperation(Operation.OPERATOR_DIV, calculation, result1);
			return Ok(new DivResponse() { Quotient = result1, Remainder = result2 });
		}

		[HttpPost]
		public IActionResult Sqrt(SqrtRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			var numbers = new List<double>() { request.Number };
			var result = Math.Sqrt(numbers.First());

			var calculation = $" {Operation.OPERATOR_SQR}{request.Number} = {result}";
			Journal.AddDataOperation(Operation.OPERATOR_SQR, calculation, result);
			return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
		}

		[HttpPost]
		public IActionResult Query(QueryRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);

			var result = Journal.GetListDataOperationById(request.Id);

			if (result == null)
			{
				var error = $"The ID {request.Id} doesn't exist. The allowed Ids are: {Environment.NewLine} All Ids: {Journal.ID_ALL_IDS} {Environment.NewLine} {string.Join(Environment.NewLine + Common.BLANK_SPACE, Journal.GetListAllIdsOperations())}";
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
