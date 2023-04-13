using System;
using System.Collections.Generic;
using System.Linq;

namespace evicalc.models
{
	public static class QueryResponse
	{
		public const string OPERATOR_ADD = "+";
		public const string OPERATOR_SUB = "-";
		public const string OPERATOR_MUL = "*";
		public const string OPERATOR_DIV = "/";
		public const string OPERATOR_SQR = "√";
		public const string OPERATOR_RESULT = "=";
		public const int ID_ALL_IDS = -1;
		public const int FIRST_ID = Common.FIRST_POSITION_ARRAY; // 0
		private static readonly IList<Record> _operations = new List<Record>() { };

		public static int GetLastId() => _operations.Count - 1;

		public static IList<int> GetListAllIdsOperations(bool idAllIdsIncluded = false)
		{
			var result = new List<int>();
			if (idAllIdsIncluded)
				result.Add(ID_ALL_IDS);

			for (int id = FIRST_ID; id <= GetLastId(); id++)
				result.Add(id);

			return result;
		}

		public static IList<Record> GetListDataOperationById(int idQuery)
		{
			if (idQuery >= FIRST_ID && idQuery <= GetLastId())
				return new List<Record>() { _operations[idQuery] };
			else
			if (idQuery == ID_ALL_IDS)
				return _operations;

			return null;
		}

		public static void AddDataOperation(string constOperation, IEnumerable<double> listNumbers, double resultOperation)
		{
			var calculation = Common.STRING_EMPTY; // ""
			var index = Common.FIRST_POSITION_ARRAY; // 0
			var numbersLength = listNumbers.Count()-1;

			foreach (double number in listNumbers)
			{
				if (constOperation == OPERATOR_SQR)
					calculation += OPERATOR_SQR;

				calculation += number + Common.BLANK_SPACE;

				// If its the last time in foreach loop
				if (index == numbersLength)
				{
					calculation += OPERATOR_RESULT + Common.BLANK_SPACE + resultOperation;

					if (constOperation == OPERATOR_DIV)
						calculation += " | Remainder: " + listNumbers.First() % listNumbers.Last();
				}
				else
				{
					calculation += constOperation + Common.BLANK_SPACE;
				}

				index++;
			}

			_operations.Add(new Record() { Operation = constOperation, Calculation = calculation, Date = DateTime.UtcNow });
		}
	}
}