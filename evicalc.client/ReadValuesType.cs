using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace evicalc.client
{
	public class ReadValuesType
	{
		public static double ReadDouble(string strRequest= "Add a valid double..", string strError= "\nThe value must be a number\n")
		{
			while (true)
			{
				Console.WriteLine(strRequest);

				var doubleInput = Console.ReadLine();
				if (double.TryParse(doubleInput, out _))
				{
					if(doubleInput.Contains(","))
						return double.Parse(doubleInput.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));

					return double.Parse(doubleInput, CultureInfo.GetCultureInfo("en-US"));
				}

				Console.WriteLine(strError);
			}
		}

		public static double? TryReadDouble(string strRequest = "Add a valid double..")
		{
			Console.WriteLine(strRequest);

			var doubleInput = Console.ReadLine();
			if (double.TryParse(doubleInput, out _))
			{
				if (doubleInput.Contains(","))
					return double.Parse(doubleInput.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));

				return double.Parse(doubleInput, CultureInfo.GetCultureInfo("en-US"));
			}
			return null;
		}

		public static int ReadInt(string strRequest = "Add a valid int..", string strError = "\nThe value must be a number without decimals\n")
		{
			while (true)
			{
				Console.WriteLine(strRequest);

				if (int.TryParse(Console.ReadLine(), out var value))
					return value;

				Console.WriteLine(strError);
			}
		}

		public static int? TryReadInt(string strRequest = "Add a valid int..")
		{
			Console.WriteLine(strRequest);

			if (int.TryParse(Console.ReadLine(), out var value))
				return value;

			return null;
		}

		public static bool ReadBoolean(string strRequest = "Do you want to exit?", string strError = "\nopps, it's not a valid value\n", string strYes = "y", string strNo="n", bool addResponsesToStrRequest = true)
		{
			if(addResponsesToStrRequest)
			{
				strRequest += $"{Environment.NewLine}'{strYes}' = Yes{Environment.NewLine}'{strNo}' = No";
			}
			while (true)
			{
				Console.WriteLine(strRequest);
				var response = Console.ReadLine();
				if (response == strYes)
					return true;
				else if(response == strNo)
					return false;
				Console.WriteLine(strError);
			}
		}

		public static char ReadChar(string strRequest = "Add a valid char..", string strError = "\nThe value must be a char\n", List<char> posibleValues = null)
		{
			while (true)
			{
				Console.WriteLine(strRequest);

				if ((char.TryParse(Console.ReadLine(), out var value) && posibleValues == null) || (posibleValues != null && posibleValues.Contains(value)))
					return value;

				Console.WriteLine(strError);
			}
		}


	}
}