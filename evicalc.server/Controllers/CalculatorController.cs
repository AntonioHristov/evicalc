using evicalc.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
			IEnumerable<double> numbers = request.Addens;
			double numberDigits = Common.getLenghtIEnumerable(numbers);
			double resultOperation;
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

			if (numberDigits == DIGITSNULL)
			{
				return BadRequest("Addens cannot be null");
			}
			else if (numberDigits < MINIMUMDIGITS)
			{
				return BadRequest("Addens param requires at least two numbers to do something");
			}
			/*
			else if(request.Addens.GetType() == typeof(string))
			{
				return BadRequest("Addens must be a number, not a text");
			}
			*/
			else
			{
				resultOperation = numbers.Sum();
				QueryResponse.addDataOperation(QueryResponse.OPERATION_ADD, numbers, resultOperation);
				return Ok(new AddResponse() { Sum = resultOperation });
			}
		}

		[HttpPost]
		//public SubResponse Sub(SubRequest request)
		public IActionResult Sub(SubRequest request)
		{
			List<double> numbers = new List<double>() { request.Minuend, request.Subtrahend };
			double resultOperation = request.Minuend - request.Subtrahend;

			QueryResponse.addDataOperation(QueryResponse.OPERATION_SUB, numbers, resultOperation);
			return Ok(new SubResponse() { Difference = resultOperation });
		}

		[HttpPost]
		public IActionResult Mult(MultRequest request)
		{
			const double digitsNull = 0;
			const double minimumDigits = 2;

			var numberDigits = request.Factors.Count();
			var numbers = request.Factors;
			var result = 1D; // Because if this is 0, not exist or has another value, the result always will be 0 after the operation

			if (numberDigits == digitsNull)
			{
				return BadRequest("Factors cannot be null");
			}
			else if (numberDigits < minimumDigits)
			{
				return BadRequest("Factors param requires at least two numbers to do something");
			}
			else
			{
				foreach (var item in numbers)
				{
					result *= item;
				}
				QueryResponse.addDataOperation(QueryResponse.OPERATION_MUL, numbers, result);
				return Ok(new MultResponse() { Product = result });
			}
		}

		[HttpPost]
		public IActionResult Div(DivRequest request)
		{
			const double illegalDivisor = 0;

			if (request.Divisor == illegalDivisor)
				return BadRequest("The divider can't be 0");

			var numbers = new List<double>() { request.Dividend, request.Divisor };
			var result = request.Dividend / request.Divisor;
			QueryResponse.addDataOperation(QueryResponse.OPERATION_DIV, numbers, result);
			return Ok(new DivResponse() { Quotient = result, Remainder = request.Dividend % request.Divisor });
		}

		[HttpPost]
		public IActionResult Sqrt(SqrtRequest request)
		{
			var numbers = new List<double>() { request.Number };
			double result = Math.Sqrt(numbers.First());

			QueryResponse.addDataOperation(QueryResponse.OPERATION_SQR, numbers, result);
			return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
		}

		[HttpPost]
		public IActionResult Query(QueryRequest request)
		{
			List <Record> result = QueryResponse.getQueryById(request.Id);

			if (result == null)
			{
				var error = $"No existe la ID {request.Id}. Las id existentes son: {Environment.NewLine} Todas las Ids: {QueryResponse.ID_ALL_IDS} {Environment.NewLine} {string.Join(Environment.NewLine, QueryResponse.getAllIdsOperations())}";
				return BadRequest(error);
			}
			else if (Common.getLenghtList(result) == Common.LENGHT_LIST_EMPTY)
			{
				var error = "Está vacío porque no has hecho ninguna operación aún...";
				return BadRequest(error);
			}
			else
			{
				return Ok(result);
			}
		}
	}
}
