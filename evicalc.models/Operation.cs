using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace evicalc.models
{
	public class Operation
	{
		public const char OPERATOR_ADD = '+';
		public const char OPERATOR_SUB = '-';
		public const char OPERATOR_MUL = '*';
		public const char OPERATOR_DIV = '/';
		public const char OPERATOR_SQR = '√';
		public const char OPERATOR_RESULT = '=';

		public const char CHAR_ADD = 'a';
		public const char CHAR_SUB = 's';
		public const char CHAR_MUL = 'm';
		public const char CHAR_DIV = 'd';
		public const char CHAR_SQR = 'q';
		public const char CHAR_RESULT = 'r';

		public const bool DIV_QUOTIENT = true;
		public const bool DIV_REMAINDER = false;

		public static double Add(IEnumerable<double> numbers)
		{
			return numbers.Sum();
		}

		public static double Sub(double minuend=0, double subtrahend=0)
		{
			return minuend - subtrahend;
		}

		public static double Mult(IEnumerable<double> numbers)
		{
			var result = 1D; // Because if this is 0, not exist or has another value, the result always will be 0 after the operation
			foreach (var item in numbers)
			{
				result *= item;
			}
			return result;
		}

		public static double? Div(double dividend = 0, double divisor = 0, bool divResult = DIV_QUOTIENT)
		{
			if (divisor == 0)
				return null;

			if (divResult == DIV_QUOTIENT)
				return dividend / divisor;
			//else if (divResult == DIV_REMAINDER) return dividend % divisor;
			return dividend % divisor;
		}

		public static double Sqrt(double number=0)
		{
			return Math.Sqrt(number);
		}

		public static IList<char> GetOperators(bool resultIncluded = false)
		{
			if(resultIncluded)
				return new List<char>() { OPERATOR_ADD, OPERATOR_SUB, OPERATOR_MUL, OPERATOR_DIV, OPERATOR_SQR, OPERATOR_RESULT };
			return new List<char>() { OPERATOR_ADD, OPERATOR_SUB, OPERATOR_MUL, OPERATOR_DIV, OPERATOR_SQR };
		}

		public static IList<char> GetChars(bool resultIncluded = false)
		{
			if (resultIncluded)
				return new List<char>() { CHAR_ADD, CHAR_SUB, CHAR_MUL, CHAR_DIV, CHAR_SQR, CHAR_RESULT };
			return new List<char>() { CHAR_ADD, CHAR_SUB, CHAR_MUL, CHAR_DIV, CHAR_SQR };
		}



	}
}
