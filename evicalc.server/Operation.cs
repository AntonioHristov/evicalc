using evicalc.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace evicalc.server
{
    public class Operation
    {
        public const bool DIV_QUOTIENT = true;
        public const bool DIV_REMAINDER = false;

		private static void _logStrOperationFile(NLog.Logger logger = null)
		{
			// Because it doesn't log the name of a model file
			Common.LogStr(Environment.NewLine + "Operation File", logger);
		}

		public static double Add(IEnumerable<double> numbers, NLog.Logger logger = null)
        {
			_logStrOperationFile(logger);
			Common.PrintCurrentMethod(logger);
            var result = numbers.Sum();
			Common.LogStr($"The result of the operation is: {result}" + Environment.NewLine, logger);
			return result;
        }

        public static double Sub(double minuend = 0, double subtrahend = 0, NLog.Logger logger = null)
        {
			_logStrOperationFile(logger);
			Common.PrintCurrentMethod(logger);
			var result = minuend - subtrahend;
			Common.LogStr($"The result of the operation is: {result}" + Environment.NewLine, logger);
			return result;
        }

        public static double Mult(IEnumerable<double> numbers, NLog.Logger logger = null)
        {
			_logStrOperationFile(logger);
			Common.PrintCurrentMethod(logger);

			var result = 1D; // Because if this is 0, not exist or has another value, the result always will be 0 after the operation
            foreach (var item in numbers)
            {
                result *= item;
            }
			Common.LogStr($"The result of the operation is: {result}" + Environment.NewLine, logger);
			return result;
        }

        public static double? Div(double dividend = 0, double divisor = 0, bool divResult = DIV_QUOTIENT, NLog.Logger logger = null)
        {
			_logStrOperationFile(logger);
			Common.PrintCurrentMethod(logger);
			if (divisor == 0)
            {
				Common.LogStr("Divisor is 0, so I return null" + Environment.NewLine, logger);
				return null;
            }
            else if (divResult == DIV_QUOTIENT)
            {
				var result1 = dividend / divisor;
				Common.LogStr($"The result of the operation is: {result1}" + Environment.NewLine, logger);
				return result1;
            }
			//else if (divResult == DIV_REMAINDER) return dividend % divisor;
			var result2 = dividend % divisor;
			Common.LogStr($"The result of the operation is: {result2}" + Environment.NewLine, logger);
			return result2;
        }

        public static double Sqrt(double number = 0, NLog.Logger logger = null)
        {
			_logStrOperationFile(logger);
			Common.PrintCurrentMethod(logger);
			var result = Math.Sqrt(number);
			Common.LogStr($"The result of the operation is: {result}" + Environment.NewLine, logger);
			return result;
        }
    }
}