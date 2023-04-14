using System;
using System.Collections.Generic;
using EasyHttp.Http;
using HttpClient = EasyHttp.Http.HttpClient;
using evicalc.models;
using System.Net;
using System.Linq.Expressions;
using System.Linq;

namespace evicalc.client
{
	public class Program
	{
		const char CODE_EXIT = 'e';

		public static void Main(string[] args)
		{
			DoOperation();
		}

		public static char OperatorRequest()
		{
			var operationsAvailable = "";

			for(var index=0; index<Operation.GetChars().Count; index++)
			{
				operationsAvailable += $"'{Operation.GetChars().ToArray()[index]}' =  {Operation.GetOperators().ToArray()[index]}{Environment.NewLine}";
			}
			operationsAvailable += $"'{CODE_EXIT}' = Exit" ;

			return ReadValuesType.ReadChar($"Tell me the operation you want to do {Environment.NewLine}{Environment.NewLine}Operations available: {Environment.NewLine}{operationsAvailable}");
			//Console.WriteLine($"Tell me the operation you want to do {Environment.NewLine}{Environment.NewLine}Operations available: {Environment.NewLine}'{string.Join("'"+Environment.NewLine+"'", Operation.GetOperators())}'{Environment.NewLine}'{Operation.OPERATOR_RESULT}' = Exit");
			//return Console.ReadLine();
		}

		public static void DoOperation()
		{
			bool exit = false;
			while (!exit)
			{
				var operatorResponse = OperatorRequest();

				switch (operatorResponse)
				{
					case Operation.CHAR_ADD:
					case Operation.OPERATOR_ADD:
						DoAddOp();
						break;
					case Operation.CHAR_SUB:
					case Operation.OPERATOR_SUB:
						DoSubOp();
						break;
					case Operation.CHAR_MUL:
					case Operation.OPERATOR_MUL:
						DoMulOp();
						break;
					case Operation.CHAR_DIV:
					case Operation.OPERATOR_DIV:
						DoDivOp();
						break;
					case Operation.CHAR_SQR:
					case Operation.OPERATOR_SQR:
						DoSqrOp();
						break;
					case CODE_EXIT:
						exit = true;
						break;
					default:
						Console.WriteLine("Value not valid");
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
			var request = new AddRequest()
			{
				Addens = new List<double>()
			};

			Console.WriteLine($"Operator: {Operation.OPERATOR_ADD} {Environment.NewLine}");

			request.Addens.Add(ReadValuesType.ReadDouble("Give me the first number"));
			request.Addens.Add(ReadValuesType.ReadDouble("Give me the second number"));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble("Do you want to add another number? Anything not a number.. skip");
				if (obj == null) break;
				request.Addens.Add(obj.Value);
			}

			// FIXME: Instead, we should log it.
			Console.WriteLine($"User input was: {string.Join(", ", request.Addens)}");

			var response = PostRequest.Post<AddRequest, AddResponse>("http://localhost:5217/Calculator/Add", request);
			Console.WriteLine("The result is: " + response.Sum + Environment.NewLine);
		}

		public static void DoSubOp()
		{
			var request = new SubRequest()
			{
				Minuend = 0D,
				Subtrahend = 0D
			};

			Console.WriteLine($"Operator: {Operation.OPERATOR_SUB} {Environment.NewLine}");
			request.Minuend = ReadValuesType.ReadDouble("Give me the minuend");
			request.Subtrahend = ReadValuesType.ReadDouble("Give me the Subtrahend");

			var response = PostRequest.Post<SubRequest, SubResponse>("http://localhost:5217/Calculator/Sub", request);
			Console.WriteLine("The result is: " + response.Difference + Environment.NewLine);
		}

		public static void DoMulOp()
		{
			var request = new MultRequest()
			{
				Factors = new List<double>()
			};

			Console.WriteLine($"Operator: {Operation.OPERATOR_MUL} {Environment.NewLine}");

			request.Factors.Add(ReadValuesType.ReadDouble("Give me the first number"));
			request.Factors.Add(ReadValuesType.ReadDouble("Give me the second number"));

			while (true)
			{
				var obj = ReadValuesType.TryReadDouble("Do you want to add another number? Anything not a number.. skip");
				if (obj == null) break;
				request.Factors.Add(obj.Value);
			}


			var response = PostRequest.Post<MultRequest, MultResponse>("http://localhost:5217/Calculator/Mult", request);
			Console.WriteLine("The result is: " + response.Product + Environment.NewLine);
		}

		public static void DoDivOp()
		{
			var request = new DivRequest()
			{
				Dividend = 0D,
				Divisor = 0D
			};

			Console.WriteLine($"Operator: {Operation.OPERATOR_DIV} {Environment.NewLine}");
			request.Dividend = ReadValuesType.ReadDouble("Give me the minuend");
			do
			{
				request.Divisor = ReadValuesType.ReadDouble("Give me the Subtrahend");
				if(request.Divisor == 0)
				{
					Console.WriteLine("Divisor can't be 0");
				}
			} while (request.Divisor == 0);


			var response = PostRequest.Post<DivRequest, DivResponse>("http://localhost:5217/Calculator/Div", request);
			Console.WriteLine("The result is: " + response.Quotient + " and the remainder is: " + response.Remainder + Environment.NewLine);
		}

		public static void DoSqrOp()
		{
			var request = new SqrtRequest()
			{
				Number = 0D
			};

			Console.WriteLine($"Operator: {Operation.OPERATOR_SQR} {Environment.NewLine}");
			request.Number = ReadValuesType.ReadDouble("Give me the number");

			var response = PostRequest.Post<SqrtRequest, SqrtResponse>("http://localhost:5217/Calculator/Sqrt", request);
			Console.WriteLine("The result is: " + response.Square + Environment.NewLine);
		}
	}
}