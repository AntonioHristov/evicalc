﻿using System.Collections.Generic;
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
		private static List<Record> _operations = new List<Record>() { };

		public static int getLastId()
		{
			return Common.getLastPositionList(getListAllDataOperations());
		}

		public static List<int> getListAllIdsOperations(bool idAllIdsIncluded = false)
		{
			var result = new List<int>();
			if (idAllIdsIncluded)
				result.Add(ID_ALL_IDS);

			for (int id = FIRST_ID; id <= getLastId(); id++)
				result.Add(id);

			return result;
		}

		public static List<Record> getListAllDataOperations()
		{
			return _operations;
		}

		public static List<Record> getListDataOperationById(int idQuery)
		{
			if (idQuery >= FIRST_ID && idQuery <= getLastId())
				return new List<Record>() { getListAllDataOperations()[idQuery] };
			else
			if (idQuery == ID_ALL_IDS)
				return getListAllDataOperations();

			return null;
		}

		public static void addDataOperation(string constOperation, IEnumerable<double> listNumbers, double resultOperation)
		{
			var calculation = Common.STRING_EMPTY; // ""
			var countForEach = Common.FIRST_POSITION_ARRAY; // 0

			foreach (double number in listNumbers)
			{
				if (constOperation == OPERATOR_SQR)
					calculation += OPERATOR_SQR;

				calculation += number;
				calculation += Common.BLANK_SPACE;

				// If its the last time in foreach loop
				if (countForEach == Common.getLastPositionIEnumerable(listNumbers))
				{
					calculation += OPERATOR_RESULT;
					calculation += Common.BLANK_SPACE;
					calculation += resultOperation;

					if (constOperation == OPERATOR_DIV)
						calculation += " | Remainder: " + listNumbers.First() % listNumbers.Last();
				}
				else
				{
					calculation += constOperation;
					calculation += Common.BLANK_SPACE;
				}
				countForEach++;
			}
			var newOperation = new Record() { Operation = constOperation, Calculation = calculation, Date = System.DateTime.Now};
			getListAllDataOperations().Add(newOperation);
		}
	}
}