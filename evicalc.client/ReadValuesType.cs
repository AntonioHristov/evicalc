using evicalc.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace evicalc.client
{
	public class ReadValuesType
	{
		private static void _logStrReadValuesFile(NLog.Logger logger = null)
		{
			// Because it doesn't log the name of a model file
			Common.LogStr(Environment.NewLine + "ReadValues File", logger);
		}

		public static double ReadDouble(string strRequest= "Add a valid double..", string strError= "\nThe value must be a number\n", NLog.Logger logger = null)
		{
			_logStrReadValuesFile(logger);
			Common.PrintCurrentMethod(logger);
			while (true)
			{
				Console.WriteLine(strRequest);

				var doubleInput = Console.ReadLine();
				if (double.TryParse(doubleInput, out _))
				{
					if(doubleInput.Contains(","))
					{
						Common.LogStr("Input decimal has , so I remplace it for . and change the culture info to en-US" + Environment.NewLine, logger);
						return double.Parse(doubleInput.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
					}
					Common.LogStr("I change the culture info to en-US" + Environment.NewLine, logger);
					return double.Parse(doubleInput, CultureInfo.GetCultureInfo("en-US"));
				}
				Common.LogStr("The User input is not a double, so I prompt it again", logger);
				Console.WriteLine(strError);
			}
		}

		public static double? TryReadDouble(string strRequest = "Add a valid double..", NLog.Logger logger = null)
		{
			_logStrReadValuesFile(logger);
			Common.PrintCurrentMethod(logger);
			Console.WriteLine(strRequest);

			var doubleInput = Console.ReadLine();
			if (double.TryParse(doubleInput, out _))
			{
				if (doubleInput.Contains(","))
				{
					Common.LogStr("Input decimal has , so I remplace it for . and change the culture info to en-US" + Environment.NewLine, logger);
					return double.Parse(doubleInput.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
				}
				Common.LogStr("I change the culture info to en-US" + Environment.NewLine, logger);
				return double.Parse(doubleInput, CultureInfo.GetCultureInfo("en-US"));
			}
			Common.LogStr("The user input is not a double, so I return a null value" + Environment.NewLine, logger);
			return null;
		}

		public static int ReadInt(string strRequest = "Add a valid int..", string strError = "\nThe value must be a number without decimals\n", NLog.Logger logger = null)
		{
			_logStrReadValuesFile(logger);
			Common.PrintCurrentMethod(logger);
			while (true)
			{
				Console.WriteLine(strRequest);

				if (int.TryParse(Console.ReadLine(), out var value))
				{
					Common.LogStr($"The user input is an int, so I return the value which is {value}" + Environment.NewLine, logger);
					return value;
				}
				Common.LogStr("The user input is not an int, so I prompt it again" + Environment.NewLine, logger);
				Console.WriteLine(strError);
			}
		}

		public static char ReadChar(string strRequest = "Add a valid char..", string strError = "\nThe value must be a char\n", List<char> posibleValues = null, NLog.Logger logger = null)
		{
			_logStrReadValuesFile(logger);
			Common.PrintCurrentMethod(logger);
			while (true)
			{
				Console.WriteLine(strRequest);

				if ((char.TryParse(Console.ReadLine(), out var value) && posibleValues == null) || (posibleValues != null && posibleValues.Contains(value)))
				{
					Common.LogStr($"The user input is a char, so I return the value which is {value}" + Environment.NewLine, logger);
					return value;
				}
				Common.LogStr("The user input is not a char, so I prompt it again" + Environment.NewLine, logger);
				Console.WriteLine(strError);
			}
		}
	}
}