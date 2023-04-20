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
using NLog.Web;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace evicalc.client
{
	public class Index
	{
		// "http://localhost:5217/Calculator/Add"
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
		private const string GENERAL_SOURCE = "./../../../";
		private const string N_LOG_SOURCE = "./../../";
		private const string JOURNAL_FILE = "journal.txt";
		private const string N_LOG_FILE = "nlogClient.config";
		private const string JOURNAL_SOURCE = GENERAL_SOURCE + JOURNAL_FILE;
		const char CODE_EXIT = 'e';
		private static OperatorMath _operatorMath = new OperatorMath();
		private static OperatorUser _operatorUser = new OperatorUser();
		private static NLog.Logger logger = NLogBuilder.ConfigureNLog(N_LOG_SOURCE + N_LOG_FILE).GetCurrentClassLogger();


		public static void Main(string[] args)
		{
			Common.PrintCurrentMethod(logger);
			SayHello();
			DoOperation();
		}

		public static void SayHello()
		{
			Common.PrintCurrentMethod(logger);
			Console.WriteLine($"Hello user,{Environment.NewLine}This is Evicalc which is my calculator program, press enter and enjoy :3");
		}

		public static void SayGoodBye()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("Clear the console, say Bye and close the program", logger);
			Console.Clear();
			Console.WriteLine("Bye");
			Console.ReadLine();
			Environment.Exit(0); // 0 is the code from exit success with no errors
		}

		public static void DoOperation()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("Creating a New ID for the user", logger);

			var exit = false;
			Journal.LogNewId(JOURNAL_SOURCE, logger);
			Common.LogStr($"Current ID: {Journal.GetCurrentID(logger)} | Last ID: {Journal.GetLastID(JOURNAL_SOURCE, logger)} (At first, current and last users have the same ID but I store both because maybe another user open the program when the current user is using it and in that case the current and last id for the first user won't be the same (I couldn't test it yet but this is the idea)", logger);

			while (!exit)
			{
				Common.LogStr("Show results and clear the console", logger);
				Console.ReadLine();
				Console.Clear();

				var operatorResponse = OperatorRequest();

				switch (operatorResponse)
				{
					case var user when user == _operatorUser._operatorAdd: // Case and when is because this fix the error "a constant value is expected" when I compare with a variable value and it works
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
						Common.LogStr("User wants to exit", logger);
						exit = true;
						break;
					default:
						Common.LogStr("The user chose an invalid operation", logger);
						Console.WriteLine($"{Environment.NewLine}Value not valid{Environment.NewLine}");
						break;
				}
			}
			SayGoodBye();
		}

		public static char OperatorRequest()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("Creating a menu with possible operations and prompt the user for one of them", logger);
			var operationsAvailable = "";

			for (var index = 0; index < _operatorMath.GetOperators().Count; index++)
			{
				operationsAvailable += $"'{_operatorUser.GetOperators().ToArray()[index]}' =  {_operatorMath.GetOperators().ToArray()[index]}{Environment.NewLine}";
			}
			operationsAvailable += $"'{_operatorUser._operatorJournal}' = Journal{Environment.NewLine}";
			operationsAvailable += $"'{CODE_EXIT}' = Exit";
			return ReadValuesType.ReadChar($"Your ID is: {Journal.GetCurrentID(logger)}{Common.GetStringManyTimes(Environment.NewLine, 2, logger)}Tell me the operation you want to do {Common.GetStringManyTimes(Environment.NewLine, 2, logger)}Operations available: {Environment.NewLine}{operationsAvailable}","", null,logger);
		}

		public static void DoAddOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Add Operation", logger);
			var request = new AddRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorAdd}");

			Common.LogStr("Prompt the numbers", logger);
			request.Addends.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the first number","", logger));
			request.Addends.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the second number","", logger));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble($"{Environment.NewLine}Do you want to add another number? Anything not a number.. skip", logger);
				if (obj == null) break;
				request.Addends.Add(obj.Value);
			}
			Common.LogStr($"User input was: {string.Join(" | ", request.Addends)}", logger);

			var response = PostRequest.Post<AddRequest, AddResponse>(POST_URL+POST_ADD, request, HttpContentTypes.ApplicationJson, logger);
			if (response != null && logger != null)
			{
				logger.Debug("The result is: {0}", response.Sum);
				Console.WriteLine($"Saved operation.{Environment.NewLine}Press enter to continue, please");
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");
		}

		public static void DoSubOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Sub Operation", logger);
			var request = new SubRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorSub}");

			Common.LogStr("Prompt the numbers", logger);
			request.Minuend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the minuend", "", logger);
			request.Subtrahend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the Subtrahend", "", logger);

			Common.LogStr($"User input was: Minuend: {request.Minuend} | Subtrahend: {request.Subtrahend}", logger);

			var response = PostRequest.Post<SubRequest, SubResponse>(POST_URL + POST_SUB, request, HttpContentTypes.ApplicationJson, logger);
			if (response != null && logger != null)
			{
				logger.Debug("The result is: {0}", response.Difference);
				Console.WriteLine($"Saved operation.{Environment.NewLine}Press enter to continue, please");
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");
		}

		public static void DoMulOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Mul Operation", logger);
			var request = new MultRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorMul}");

			Common.LogStr("Prompt the numbers", logger);
			request.Factors.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the first number", "", logger));
			request.Factors.Add(ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the second number", "", logger));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble($"{Environment.NewLine}Do you want to add another number? Anything not a number.. skip", logger);
				if (obj == null) break;
				request.Factors.Add(obj.Value);
			}
			Common.LogStr($"User input was: {string.Join(" | ", request.Factors)}", logger);

			var response = PostRequest.Post<MultRequest, MultResponse>(POST_URL + POST_MULT, request, HttpContentTypes.ApplicationJson, logger);
			if (response != null && logger != null)
			{
				logger.Debug("The result is: {0}", response.Product);
				Console.WriteLine($"Saved operation.{Environment.NewLine}Press enter to continue, please");
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");
		}

		public static void DoDivOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Div Operation", logger);
			var request = new DivRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorDiv}");

			Common.LogStr("Prompt the numbers", logger);
			request.Dividend = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the minuend", "", logger);

			do
			{
				request.Divisor = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the Subtrahend", "", logger);
				if(request.Divisor == 0)
				{
					Common.LogStr("The user give a 0 in the divisor, so I prompt the divisor again...", logger);
					Console.WriteLine($"{Environment.NewLine}Divisor can't be 0");
				}
			} while (request.Divisor == 0);

			Common.LogStr($"User input was: Dividend: {request.Dividend} | Divisor: {request.Divisor}", logger);

			var response = PostRequest.Post<DivRequest, DivResponse>(POST_URL + POST_DIV, request, HttpContentTypes.ApplicationJson, logger);
			if (response != null && logger != null)
			{
				logger.Debug("The quotient is: {0} and the remainder is: {1}", response.Quotient, response.Remainder);
				Console.WriteLine($"Saved operation.{Environment.NewLine}Press enter to continue, please");
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");
		}

		public static void DoSqrOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Sqr Operation", logger);
			var request = new SqrtRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: {_operatorMath._operatorSqr}");

			Common.LogStr("Prompt the number", logger);
			request.Number = ReadValuesType.ReadDouble($"{Environment.NewLine}Give me the number", "", logger);

			var response = PostRequest.Post<SqrtRequest, SqrtResponse>(POST_URL + POST_SQRT, request, HttpContentTypes.ApplicationJson, logger);
			if (response != null && logger != null)
			{
				logger.Debug("The result is: {0}", response.Square);
				Console.WriteLine($"Saved operation.{Environment.NewLine}Press enter to continue, please");
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");
		}

		public static void DoJournalOp()
		{
			Common.PrintCurrentMethod(logger);
			Common.LogStr("The user chose The Journal Operation", logger);
			var request = new QueryRequest();

			Console.WriteLine($"{Environment.NewLine}Operator: Journal");

			Common.LogStr("Prompt the Journal ID", logger);
			request.Id  = ReadValuesType.ReadInt($"{Environment.NewLine}Give me the ID, All Ids: {Journal.ID_ALL_IDS}{Environment.NewLine}{string.Join(Environment.NewLine, Journal.GetListAllIdsOperations(false, JOURNAL_SOURCE, logger))}", "", logger);
			var response = PostRequest.Post<QueryRequest, QueryResponse>(POST_URL + POST_QUERY, request, HttpContentTypes.ApplicationJson, logger);

			if (response != null && logger != null)
			{
				logger.Debug("The result is: {0}", Journal.ParseListRecordToString(response.Operations, logger));
				Console.WriteLine(Journal.ParseListRecordToString(response.Operations));
			}
			else if (logger != null)
				logger.Debug("Error with the operation, response is null");

			if (logger != null)
				logger.Debug("2 New Lines");
			Common.LogStr("2 New Lines", logger);
			Console.WriteLine(Common.GetStringManyTimes(Environment.NewLine, 2, logger));
		}
	}
}