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

        public static double Add(IEnumerable<double> numbers)
        {
            return numbers.Sum();
        }

        public static double Sub(double minuend = 0, double subtrahend = 0)
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

        public static double Sqrt(double number = 0)
        {
            return Math.Sqrt(number);
        }
    }
}