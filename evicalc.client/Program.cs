using System;
using System.Collections.Generic;
using EasyHttp.Http;
using HttpClient = EasyHttp.Http.HttpClient;
using evicalc.models;
using System.Net;
using System.Linq.Expressions;
using System.Linq;
using EasyHttp.Infrastructure;
using System.Text;
using System.IO;

namespace evicalc.client
{
	public class Program
	{
		//"http://localhost:5217/Calculator/Add"
		private const string POST_PROTOCOL = "http";
		private const string POST_IP = "localhost";
		private const string POST_PORT = "5217";
		private const string POST_CONTROLLER = "Calculator";
		private const string POST_ADD = "Add";
		private const string POST_SUB = "Sub";
		private const string POST_MULT = "Mult";
		private const string POST_DIV = "Div";
		private const string POST_SQRT = "Sqrt";
		private const string POST_QUERY = "Query";
		private const string POST_URL = POST_PROTOCOL+"://"+POST_IP+":"+POST_PORT+"/"+POST_CONTROLLER+"/";
		private const string FILE_SOURCE = "./../../../log.txt";
		const char CODE_EXIT = 'e';
		private static OperatorMath _operatorMath = new OperatorMath();
		private static OperatorUser _operatorUser = new OperatorUser();

		public static void Main(string[] args)
		{
			DoOperation();
		}

		public static char OperatorRequest()
		{
			var operationsAvailable = "";

			for (var index = 0; index < _operatorMath.GetOperators().Count; index++)
			{
				operationsAvailable += $"'{_operatorUser.GetOperators().ToArray()[index]}' =  {_operatorMath.GetOperators().ToArray()[index]}{Environment.NewLine}";
			}
			operationsAvailable += $"'{_operatorUser._operatorJournal}' = Journal{Environment.NewLine}";
			operationsAvailable += $"'{CODE_EXIT}' = Exit" ;
			return ReadValuesType.ReadChar($"Tell me the operation you want to do {Environment.NewLine}{Environment.NewLine}Operations available: {Environment.NewLine}{operationsAvailable}");
		}

		public static void DoOperation()
		{
			var exit = false;
			Journal.LogNewId(FILE_SOURCE);

			while (!exit)
			{
				var operatorResponse = OperatorRequest();

				switch (operatorResponse)
				{
					case var user when user == _operatorUser._operatorAdd: // Because this fix the error "a constant value is expected" when I compare with a variable value and it works
					case var math when math == _operatorMath._operatorAdd:
						DoAddOp();
						break;
					case var user when user == _operatorUser._operatorSub:
					case var math when math == _operatorMath._operatorSub:
						DoSubOp();
						break;
					case var user when user == _operatorUser._operatorMul:
					case var math when math == _operatorMath._operatorMul:
						DoMulOp();
						break;
					case var user when user == _operatorUser._operatorDiv:
					case var math when math == _operatorMath._operatorDiv:
						DoDivOp();
						break;
					case var user when user == _operatorUser._operatorSqr:
					case var math when math == _operatorMath._operatorSqr:
						DoSqrOp();
						break;
					case var user when user == _operatorUser._operatorJournal:
						DoJournalOp();
						break;
					case CODE_EXIT:
						exit = true;
						break;
					default:
						Console.WriteLine($"{Environment.NewLine}Value not valid{Environment.NewLine}");
						break;
				}
			}
			Console.Clear();
			Console.WriteLine("Bye");
			Console.ReadLine();
			Environment.Exit(0); // 0 is the code from exit success with no errors
		}

		public static void DoAddOp()
		{
			var request = new AddRequest(){Addens = new List<double>()};

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorAdd}");

			request.Addens.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the first number"));
			request.Addens.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the second number"));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble($"{Environment.NewLine}Do you want to add another number? Anything not a number.. skip");
				if (obj == null) break;
				request.Addens.Add(obj.Value);
			}

			// FIXME: Instead, we should log it.
			Console.WriteLine($"{Environment.NewLine}User input was: {string.Join(", ", request.Addens)}");

			var response = PostRequest.Post<AddRequest, AddResponse>(POST_URL+POST_ADD, request);
			if (response != null)
				Console.WriteLine($"{Environment.NewLine}The result is: {response.Sum}{Environment.NewLine}");
		}

		public static void DoSubOp()
		{
			var request = new SubRequest(){Minuend = 0D,Subtrahend = 0D};

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorSub}");
			request.Minuend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the minuend");
			request.Subtrahend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the Subtrahend");

			var response = PostRequest.Post<SubRequest, SubResponse>(POST_URL + POST_SUB, request);
			if (response != null)
				Console.WriteLine($"{Environment.NewLine}The result is: {response.Difference}{Environment.NewLine}");
		}

		public static void DoMulOp()
		{
			var request = new MultRequest(){Factors = new List<double>()};

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorMul}");

			request.Factors.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the first number"));
			request.Factors.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the second number"));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble($"{Environment.NewLine}Do you want to add another number? Anything not a number.. skip");
				if (obj == null) break;
				request.Factors.Add(obj.Value);
			}


			var response = PostRequest.Post<MultRequest, MultResponse>(POST_URL + POST_MULT, request);
			if (response != null)
				Console.WriteLine($"{Environment.NewLine}The result is: {response.Product}{Environment.NewLine}");
		}

		public static void DoDivOp()
		{
			var request = new DivRequest(){Dividend = 0D,Divisor = 0D};

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorDiv}");
			request.Dividend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the minuend");
			/*
			do
			{
				request.Divisor = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the Subtrahend");
				if(request.Divisor == 0)
				{
					Console.WriteLine($"{Environment.NewLine}Divisor can't be 0");
				}
			} while (request.Divisor == 0);
			*/request.Divisor = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the Subtrahend");

			var response = PostRequest.Post<DivRequest, DivResponse>(POST_URL + POST_DIV, request);
			if (response != null)
				Console.WriteLine($"{Environment.NewLine}The result is: {response.Quotient} and the remainder is: {response.Remainder}{Environment.NewLine}");
		}

		public static void DoSqrOp()
		{
			var request = new SqrtRequest(){Number = 0D};

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorSqr}");
			request.Number = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the number");

			var response = PostRequest.Post<SqrtRequest, SqrtResponse>(POST_URL + POST_SQRT, request);
			if (response != null)
				Console.WriteLine($"{Environment.NewLine}The result is: {response.Square}{Environment.NewLine}");
		}

		public static void DoJournalOp()
		{
			var request = new QueryRequest(); //{ Id = 0 };
			var sb = new StringBuilder();


			Console.WriteLine($"{Environment.NewLine}Operator: Journal");
			request.Id  = ReadValuesType.ReadInt($"{Environment.NewLine}Give me the ID, All Ids: {Journal.ID_ALL_IDS} {Environment.NewLine} {string.Join(Environment.NewLine + Common.BLANK_SPACE, Journal.GetListAllIdsOperations(false, FILE_SOURCE))}");

			var response = PostRequest.Post<QueryRequest, QueryResponse>(POST_URL + POST_QUERY, request);

			if (response != null)
				Console.WriteLine(Journal.ParseListRecordToString(response.Operations));
			Console.WriteLine(Environment.NewLine + Environment.NewLine);
		}

		public static void ReadFile()
		{
			IEnumerable<string> lines = File.ReadLines(FILE_SOURCE);
			Console.WriteLine(String.Join(Environment.NewLine, lines));
		}

	}
}