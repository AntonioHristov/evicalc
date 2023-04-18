using System;
using System.Collections;
using System.IO;

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
			return double.TryParse(valueString, out double doubleValue);
		}

		public static bool StringIsDouble(string valueString, out double result)
		{
			return double.TryParse(valueString, out result);
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

		public static bool CheckFileExist(string source = "", bool createIfNot = false)
		{
			if (!File.Exists(source))
			{ // Create a file to write to
				if (createIfNot)
					File.Create(source).Close();
				else
					return false;
			}
			return true;
		}
	}
}
