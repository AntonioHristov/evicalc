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

		public static bool CheckFileExist(string source = "", bool createIfNot = false, NLog.Logger logger = null)
		{
			_logStrCommonFile(logger);
			PrintCurrentMethod(logger);
			if (!File.Exists(source))
			{
				LogStr($"The file not exist, the source is: {source}" + Environment.NewLine, logger);
				if (createIfNot)
				{
					LogStr("But I create it, so now the file exist" + Environment.NewLine, logger);
					File.Create(source).Close();
				}
				else
					return false;
			}
			LogStr($"The file exist, the source is: {source}" + Environment.NewLine, logger);
			return true;
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
