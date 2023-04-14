﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace evicalc.client
{
	public class ReadValuesType
	{
		public static double ReadDouble(string strRequest= "Add a valid double..", string strError= "The value must be a number\n")
		{
			while (true)
			{
				Console.WriteLine(strRequest);

				if (double.TryParse(Console.ReadLine(), out var value))
					return value;

				Console.WriteLine(strError);
			}
		}

		public static double? TryReadDouble(string strRequest = "Add a valid double..")
		{
				Console.WriteLine(strRequest);

				if (double.TryParse(Console.ReadLine(), out var value))
					return value;

				return null;
		}

		public static bool ReadBoolean(string strRequest = "Do you want to exit?", string strError = "opps, it's not a valid value\n", string strYes = "y", string strNo="n", bool addResponsesToStrRequest = true)
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

		public static char ReadChar(string strRequest = "Add a valid char..", string strError = "The value must be a char\n", List<char> posibleValues = null)
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