
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
		private const string GENERAL_SOURCE = "./../";
		private const string N_LOG_SOURCE = "./../../../";
		private const string JOURNAL_FILE = "journal.txt";
		private const string N_LOG_FILE = "nlogServer.config";
		private const string JOURNAL_SOURCE = GENERAL_SOURCE+JOURNAL_FILE;
		private static Operator _operatorMath = new OperatorMath();
		private static NLog.Logger logger = NLogBuilder.ConfigureNLog(N_LOG_SOURCE + N_LOG_FILE).GetCurrentClassLogger();

		private readonly ILogger<CalculatorController> _logger;

		#endregion

		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public IActionResult Add(AddRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Add Operation", logger);

			const double MINIMUMDIGITS = 2;
			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Addens?.Any() != true) //< Null or empty..
			{
				Common.LogStr("Error, the content of request is null", logger);
				return BadRequest("Addens cannot be null or empty");
			}
			else if (request.Addens.GetType() != typeof(List<double>))
			{
				Common.LogStr("Error, the content of request doesn't have the right type of value", logger);
				return BadRequest("Addens must be a list of numbers");
			}
			else if (request.Addens.Count < MINIMUMDIGITS)
			{
				Common.LogStr("Error, the content of request is not enough", logger);
				return BadRequest("Addens param requires at least two numbers to do something");
			}

			Common.LogStr("Calculating", logger);
			var result = Operation.Add(request.Addens); // request.Addens.Sum(); This case is simple but other cases like mult or div are not so simple like this
			var calculation = $"{string.Join(" + ", request.Addens)} = {result}"; //< ie. "N + Y = X"

			Common.LogStr("Add the operation in Journal", logger);
			Journal.AddDataOperation(_operatorMath._operatorAdd, calculation, Journal._dateNow, JOURNAL_SOURCE, logger);

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new AddResponse() { Sum = result });
		}

		[HttpPost]
		public IActionResult Sub(SubRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Sub Operation", logger);

			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Minuend.GetType() != typeof(double))
			{
				Common.LogStr("Error, the minuend of request doesn't have the right type of value", logger);
				return BadRequest("Minuend must be a number");
			}
			else if (request.Subtrahend.GetType() != typeof(double))
			{
				Common.LogStr("Error, the Subtrahend of request doesn't have the right type of value", logger);
				return BadRequest("Subtrahend must be a number");
			}

			Common.LogStr("Calculating", logger);

			var numbers = new List<double>() { request.Minuend, request.Subtrahend };
			var result = Operation.Sub(request.Minuend, request.Subtrahend);
			var calculation = $"{string.Join(" - ", numbers)} = {result}";

			Common.LogStr("Add the operation in Journal", logger);
			Journal.AddDataOperation(_operatorMath._operatorSub, calculation, Journal._dateNow, JOURNAL_SOURCE, logger);

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new SubResponse() { Difference = result });
		}

		[HttpPost]
		public IActionResult Mult(MultRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Mult Operation", logger);

			const double minimumDigits = 2;
			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Factors?.Any() != true)
			{
				Common.LogStr("Error, the content of request is null", logger);
				return BadRequest("Factors cannot be null");
			}
			else if (request.Factors.GetType() != typeof(List<double>))
			{
				Common.LogStr("Error, the content of request doesn't have the right type of value", logger);
				return BadRequest("Factors must be a list of numbers");
			}
			else if (request.Factors.Count() < minimumDigits)
			{
				Common.LogStr("Error, the content of request is not enough", logger);
				return BadRequest("Factors param requires at least two numbers to do something");
			}

			Common.LogStr("Calculating", logger);
			var result = Operation.Mult(request.Factors);
			var calculation = $"{string.Join(" * ", request.Factors)} = {result}";

			Common.LogStr("Add the operation in Journal", logger);
			Journal.AddDataOperation(_operatorMath._operatorMul, calculation, Journal._dateNow, JOURNAL_SOURCE, logger);

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new MultResponse() { Product = result });
		}

		[HttpPost]
		public IActionResult Div(DivRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Div Operation", logger);

			const double illegalDivisor = 0;
			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Dividend.GetType() != typeof(double))
			{
				Common.LogStr("Error, the Dividend of request doesn't have the right type of value", logger);
				return BadRequest("Dividend must be a number");
			}
			else if (request.Divisor.GetType() != typeof(double))
			{
				Common.LogStr("Error, the Divisor of request doesn't have the right type of value", logger);
				return BadRequest("Divisor must be a number");
			}
			else if (request.Divisor == illegalDivisor)
			{
				Common.LogStr("Error, the Divisor of request is 0", logger);
				return BadRequest("Divisor can't be 0");
			}

			Common.LogStr("Calculating", logger);
			var numbers = new List<double>() { request.Dividend, request.Divisor };
			var resultQuotient = Operation.Div(request.Dividend, request.Divisor, Operation.DIV_QUOTIENT);
			var resultRemainder = Operation.Div(request.Dividend, request.Divisor, Operation.DIV_REMAINDER);
			var calculation = $"{string.Join(" / ", numbers)} = {resultQuotient} | {string.Join(" % ", numbers)} = {resultRemainder}";

			Common.LogStr("Add the operation in Journal", logger);
			Journal.AddDataOperation(_operatorMath._operatorDiv, calculation, Journal._dateNow, JOURNAL_SOURCE, logger);

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new DivResponse() { Quotient = resultQuotient.Value, Remainder = resultRemainder.Value });
		}

		[HttpPost]
		public IActionResult Sqrt(SqrtRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Sqrt Operation", logger);

			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Number.GetType() != typeof(double))
			{
				Common.LogStr("Error, the Number of request doesn't have the right type of value", logger);
				return BadRequest("Number must be a number");
			}

			Common.LogStr("Calculating", logger);
			var result = Operation.Sqrt(request.Number);
			var calculation = $" {_operatorMath._operatorSqr}{request.Number} = {result}";

			Common.LogStr("Add the operation in Journal", logger);
			Journal.AddDataOperation(_operatorMath._operatorSqr, calculation, Journal._dateNow, JOURNAL_SOURCE, logger);

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new SqrtResponse() { Square = Math.Sqrt(request.Number) });
		}

		[HttpPost]
		public IActionResult Query(QueryRequest request)
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Query/Journal Operation", logger);

			if (request == null)
			{
				Common.LogStr("Error, the request is null", logger);
				return BadRequest(REQUEST_CANNOT_BE_NULL);
			}
			else if (request.Id.GetType() != typeof(int))
			{
				Common.LogStr("Error, the Id of request doesn't have the right type of value", logger);
				return BadRequest("Id must be a number, an int");
			}

			Common.LogStr("Calculating", logger);
			var resultList = Journal.GetListDataOperationByIdList(request.Id, JOURNAL_SOURCE, logger);

			if (resultList == null)
			{
				Common.LogStr("The ID doesn't exist", logger);
				var error = $"The ID {request.Id} doesn't exist. The allowed Ids are: {Environment.NewLine}All Ids: {Journal.ID_ALL_IDS}{Environment.NewLine}{string.Join(Environment.NewLine, Journal.GetListAllIdsOperations(false, JOURNAL_SOURCE, logger))}";
				return BadRequest(error);
			}
			else if (resultList.Count == Common.FIRST_POSITION_ARRAY)
			{
				Common.LogStr("It's empty, there's no operations", logger);
				var error = "It's empty, there's no operations";
				return BadRequest(error);
			}

			Common.LogStr("Return Ok to the client with the result", logger);
			return Ok(new QueryResponse() { Operations = (List<Record>)resultList });
		}
	}
}
