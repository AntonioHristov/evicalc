using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace evicalc.models
{
	public class Common
	{
		public const string STRING_EMPTY = "";
		public const string BLANK_SPACE = " ";
		public const int FIRST_POSITION_ARRAY = 0;
		public const int LENGHT_LIST_EMPTY = 0;

		private static void _logStrCommonFile(NLog.Logger logger = null)
		{
			// Because it doesn't log the name of a model file
			Common.LogStr(Environment.NewLine + "Common File", logger);
		}

		public static void LogStr(string text = "", NLog.Logger logger = null)
		{
			if (logger != null)
				logger.Debug(text);
		}

		public static string GetStringManyTimes(string text="", int times=1, NLog.Logger logger = null)
		{
			_logStrCommonFile(logger);
			PrintCurrentMethod(logger);
			var result = "";
			for(int count = 0; count<times; count++)
			{
				result += text;
			}
			LogStr($"The string result is: {result}" + Environment.NewLine, logger);
			return result;
		}
		// Continue here if I can...
		public static bool StringIsDouble(string valueString, NLog.Logger logger = null)
		{
			PrintCurrentMethod(logger);
			return double.TryParse(valueString, out double doubleValue);
		}

		public static bool StringIsDouble(string valueString, out double result, NLog.Logger logger = null)
		{
			PrintCurrentMethod(logger);
			return double.TryParse(valueString, out result);
		}

		public static bool ArrayContainsChar(char caracter=' ', IList<char> arrayChar = null, NLog.Logger logger = null)
		{
			PrintCurrentMethod(logger);
			foreach (var item in arrayChar)
			{
				if (item == caracter)
				{
					return true;
				}
			}
			return false;
		}

		public static bool CheckFileExist(string source = "", bool createIfNot = false, NLog.Logger logger = null)
		{
			PrintCurrentMethod(logger);
			if (!File.Exists(source))
			{
				if (logger != null)
					logger.Debug("The file not exist, the source is: {0}", source);
				if (createIfNot)
				{
					if (logger != null)
						logger.Debug("But I create it, so now the file exist");
					File.Create(source).Close();
				}
				else
					return false;
			}
			LogStr($"The file exist, the source is: {source}", logger);
			return true;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string GetCurrentMethod(NLog.Logger logger = null)
		{
			PrintCurrentMethod(logger);
			var st = new StackTrace();
			var sf = st.GetFrame(1);

			return sf.GetMethod().Name;
		}

		public static void PrintCurrentMethod(NLog.Logger logger = null, string before = "", string after = " method")
		{
			if (logger != null)
			{
				var st = new StackTrace();
				var sf = st.GetFrame(1);
				logger.Debug(before + sf.GetMethod().Name + after);
			}
		}

	}
}
