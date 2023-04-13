using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using EasyHttp.Http;
using evicalc.models;
using HttpClient = EasyHttp.Http.HttpClient;

namespace evicalc.client
{
	public class Program
	{
		const string OPERATOR_ADD = "+";
		const string OPERATOR_SUB = "-";
		const string OPERATOR_MUL = "*";
		const string OPERATOR_DIV = "/";
		const string OPERATOR_SQR = "v";
		const string OPERATOR_RESULT = "=";

		public static void Main(string[] args)
		{
			Console.WriteLine("Ejecutando cliente en consola");
			DoOperation();
			Console.ReadLine();
		}

		public static string OperatorRequest()
		{
			var operationsList = new List<string>() { OPERATOR_ADD, OPERATOR_SUB, OPERATOR_MUL, OPERATOR_DIV, OPERATOR_SQR };

			// Remember to create here a Do While, validating the user value
			Console.WriteLine($"Tell me the operation you want to do {Environment.NewLine}Operations available: {Environment.NewLine}{string.Join(Environment.NewLine, operationsList)}{Environment.NewLine}'{OPERATOR_RESULT}' = (Exit)");
			return Console.ReadLine();
		}

		public static void DoOperation()
		{
			if (OperatorRequest() == OPERATOR_ADD)
			{
				var request = new AddRequest
				{
					Addens = new List<double>()
				};
				var lastInput = "";

				Console.WriteLine($"Operator: {OPERATOR_ADD} {Environment.NewLine}Give me the numbers, one by one.{Environment.NewLine}If you want to stop the operation, write '{OPERATOR_RESULT}'");
				do
				{
					lastInput = Console.ReadLine();
					if (Common.StringIsDouble(lastInput))
					{
						request.Addens.Add(double.Parse(lastInput));
						Console.WriteLine("Number added");
					}
					else if(lastInput != OPERATOR_RESULT)
						Console.WriteLine("Number not added");

				} while ( Common.StringIsDouble(lastInput) || lastInput != OPERATOR_RESULT );

				Console.WriteLine("Values: ");
                foreach (var item in request.Addens)
                {
					Console.WriteLine(item);
				}

				var http = new HttpClient();
				//http.Post("url", customer, HttpContentTypes.ApplicationJson);
				http.Post("url", request, "Add");


			}
		}
	}
}