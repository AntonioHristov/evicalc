using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace evicalc.models
{
	public class OperatorUser : Operator
	{
		public char _operatorJournal { get; set; }

		private const char OPERATOR_ADD = 'a';
		private const char OPERATOR_SUB = 's';
		private const char OPERATOR_MUL = 'm';
		private const char OPERATOR_DIV = 'd';
		private const char OPERATOR_SQR = 'q';
		private const char OPERATOR_RESULT = 'r';
		private const char OPERATOR_JOURNAL = 'j';

		public OperatorUser() : base(OPERATOR_ADD, OPERATOR_SUB, OPERATOR_MUL, OPERATOR_DIV, OPERATOR_SQR, OPERATOR_RESULT)
		{
			_operatorJournal = OPERATOR_JOURNAL;
		}
	}
}
