
using evicalc.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;

namespace evicalc.server.Controllers
{
    [ApiController]
	[Route("[controller]/[action]")]
	public class CalculatorController : ControllerBase
	{
		#region Fields

		private const string REQUEST_CANNOT_BE_NULL = "Invalid request received, look like cannot be deserialized properly";
		private static Operator _operatorMath = new OperatorMath();
		private const string GENERAL_SOURCE = "./../";
		private const string N_LOG_SOURCE = "./../../../../";
		private const string JOURNAL_FILE = "journal.txt";
		private const string LOG_FILE = "log.log";
		private const string N_LOG_FILE = "nlog.config";
		private const string JOURNAL_SOURCE = GENERAL_SOURCE+JOURNAL_FILE;
		private const string LOG_SOURCE = GENERAL_SOURCE + LOG_FILE;

		private readonly ILogger<CalculatorController> _logger;

		#endregion

		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Add(AddRequest request)
		{
			var logger = NLogBuilder.ConfigureNLog(N_LOG_SOURCE + N_LOG_FILE).GetCurrentClassLogger();
			logger.Debug("Hola Mundo Servidor");

			const double MINIMUMDIGITS = 2;
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Addens?.Any() != true) //< Null or empty..
				return BadRequest("Addens cannot be null or empty");
			else if (request.Addens.GetType() != typeof(List<double>))
				return BadRequest("Addens must be a list of numbers");
			else if (request.Addens.Count < MINIMUMDIGITS)
				return BadRequest("Addens param requires at least two numbers to do something");

			var result = Operation.Add(request.Addens); // request.Addens.Sum(); This case is simple but other cases like mult or div are not so simple like this
			var calculation = $"{string.Join(" + ", request.Addens)} = {result}"; //< ie. "N + Y = X"
			Journal.AddDataOperation(_operatorMath._operatorAdd, calculation, Journal._dateNow, JOURNAL_SOURCE);
			return Ok(new AddResponse() { Sum = result });
		}

		[HttpPost]
		public IActionResult Sub(SubRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Minuend.GetType() != typeof(double))
				return BadRequest("Minuend must be a number");
			else if (request.Subtrahend.GetType() != typeof(double))
				return BadRequest("Subtrahend must be a number");

			var numbers = new List<double>() { request.Minuend, request.Subtrahend };
			var result = Operation.Sub(request.Minuend, request.Subtrahend);
			var calculation = $"{string.Join(" - ", numbers)} = {result}";
			Journal.AddDataOperation(_operatorMath._operatorSub, calculation, Journal._dateNow, JOURNAL_SOURCE);
			return Ok(new SubResponse() { Difference = result });
		}

		[HttpPost]
		public IActionResult Mult(MultRequest request)
		{
			const double minimumDigits = 2;
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Factors == null)
				return BadRequest("Factors cannot be null");
			else if (request.Factors.Count() < minimumDigits)
				return BadRequest("Factors param requires at least two numbers to do something");

			var result = Operation.Mult(request.Factors);
			var calculation = $"{string.Join(" * ", request.Factors)} = {result}";
			Journal.AddDataOperation(_operatorMath._operatorMul, calculation, Journal._dateNow, JOURNAL_SOURCE);
			return Ok(new MultResponse() { Product = result });
		}

		[HttpPost]
		public IActionResult Div(DivRequest request)
		{
			const double illegalDivisor = 0;
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Dividend.GetType() != typeof(double))
				return BadRequest("Dividend must be a number");
			else if (request.Divisor.GetType() != typeof(double))
				return BadRequest("Divisor must be a number");
			else if (request.Divisor == illegalDivisor)
				return BadRequest("Divisor can't be 0");

			var numbers = new List<double>() { request.Dividend, request.Divisor };
			var resultQuotient = Operation.Div(request.Dividend, request.Divisor, Operation.DIV_QUOTIENT);
			var resultRemainder = Operation.Div(request.Dividend, request.Divisor, Operation.DIV_REMAINDER);
			var calculation = $"{string.Join(" / ", numbers)} = {resultQuotient} | {string.Join(" % ", numbers)} = {resultRemainder}";
			Journal.AddDataOperation(_operatorMath._operatorDiv, calculation, Journal._dateNow, JOURNAL_SOURCE);
			return Ok(new DivResponse() { Quotient = resultQuotient.Value, Remainder = resultRemainder.Value });
		}

		[HttpPost]
		public IActionResult Sqrt(SqrtRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Number.GetType() != typeof(double))
				return BadRequest("Number must be a number");

			var result = Operation.Sqrt(request.Number);
			var calculation = $" {_operatorMath._operatorSqr}{request.Number} = {result}";
			Journal.AddDataOperation(_operatorMath._operatorSqr, calculation, Journal._dateNow, JOURNAL_SOURCE);
			return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
		}

		[HttpPost]
		public IActionResult Query(QueryRequest request)
		{
			if (request == null)
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			else if (request.Id.GetType() != typeof(int))
				return BadRequest("Id must be a number, an int");

			var resultList = Journal.GetListDataOperationByIdList(request.Id, JOURNAL_SOURCE);

			if (resultList == null)
			{
				var error = $"The ID {request.Id} doesn't exist. The allowed Ids are: {Environment.NewLine}All Ids: {Journal.ID_ALL_IDS}{Environment.NewLine}{string.Join(Environment.NewLine, Journal.GetListAllIdsOperations(false, JOURNAL_SOURCE))}";
				return BadRequest(error);
			}
			else if (resultList.Count == Common.FIRST_POSITION_ARRAY)
			{
				var error = "It's empty, there's no operations";
				return BadRequest(error);
			}

			var result = new QueryResponse() { Operations = (List<Record>)resultList };
			return Ok(result);
		}
	}
}
