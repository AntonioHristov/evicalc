using System;
using System.Collections;

namespace evicalc.models
{
	public class Common
	{
		public const string STRING_EMPTY = "";
		public const string BLANK_SPACE = " ";
		public const int FIRST_POSITION_ARRAY = 0;
		public const int LENGHT_LIST_EMPTY = 0;

		public static string GetStringManyTimes(string text="", int times=1 )
		{
			var result = "";
			for(int count = 0; count<times; count++)
			{
				result += text;
			}
			return result;
		}

		public static bool StringIsDouble(string valueString)
		{
			double variableDouble;
			return Double.TryParse(valueString, out variableDouble);
		}

		public static bool ArrayContainsChar(char caracter, params char[] arrayChar)
		{
			foreach (var item in arrayChar)
			{
				if (item == caracter)
				{
					return true;
				}
			}
			return false;
		}
	}
}
