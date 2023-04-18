using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace evicalc.models
{
	public class OperatorMath : Operator
	{
		private const char OPERATOR_ADD = '+';
		private const char OPERATOR_SUB = '-';
		private const char OPERATOR_MUL = '*';
		private const char OPERATOR_DIV = '/';
		private const char OPERATOR_SQR = '√';
		private const char OPERATOR_RESULT = '=';

		public OperatorMath() : base(OPERATOR_ADD, OPERATOR_SUB, OPERATOR_MUL, OPERATOR_DIV, OPERATOR_SQR, OPERATOR_RESULT){}
	}
}
