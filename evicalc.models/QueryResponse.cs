using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace evicalc.models
{
	public static class QueryResponse
	{
		public const string OPERATION_ADD = "Sum";
		public const string OPERATION_SUB = "Sub";
		public const string OPERATION_MUL = "Mul";
		public const string OPERATION_DIV = "Div";
		public const string OPERATION_SQR = "Sqr";
		public const int ID_ALL_IDS = -1;
		public const int FIRST_ID = Common.FIRST_POSITION_ARRAY; // 0


		private static List<Record> _operations = new List<Record>() { };



		public static int getLastId()
		{
			return Common.getLastPositionList(getAllDataOperations());
		}

		public static List<int> getAllIdsOperations(bool idAllIdsIncluded = false)
		{
			var result = new List<int>();
			if (idAllIdsIncluded)
			{
				result.Add(ID_ALL_IDS);
			}
			for (int id = FIRST_ID; id <= getLastId(); id++)
			{
				result.Add(id);
			}
			return result;
		}

		public static List<Record> getAllDataOperations()
		{
			return _operations;
		}

		public static void addDataOperation(string operation, IEnumerable<double> numbers, double result)
		{
			const string OPERATOR_ADD_STRING_CALCULATION = "+";
			const string OPERATOR_SUB_STRING_CALCULATION = "-";
			const string OPERATOR_MUL_STRING_CALCULATION = "*";
			const string OPERATOR_DIV_STRING_CALCULATION = "/";
			const string OPERATOR_SQR_STRING_CALCULATION = "√";
			const string OPERATOR_RESULT_STRING_CALCULATION = "=";
			string calculation = Common.STRING_EMPTY; // ""
			Record newOperation;


			int countForEach = Common.FIRST_POSITION_ARRAY; // 0
			foreach (double number in numbers)
			{
				if (operation == OPERATION_SQR)
				{
					calculation += OPERATOR_SQR_STRING_CALCULATION;
				}
				calculation += number;
				calculation += Common.BLANK_SPACE;

				// If its the last time in foreach loop
				if (countForEach == Common.getLastPositionIEnumerable(numbers))
				{
					calculation += OPERATOR_RESULT_STRING_CALCULATION;
					calculation += Common.BLANK_SPACE;
					calculation += result;

					if (operation == OPERATION_DIV)
					{
						calculation += " | Remainder: " + numbers.First() % numbers.Last();
					}
				}
				else
				{
					if (operation == OPERATION_ADD)
					{
						calculation += OPERATOR_ADD_STRING_CALCULATION;
					}
					else
					if (operation == OPERATION_SUB)
					{
						calculation += OPERATOR_SUB_STRING_CALCULATION;
					}
					else
					if (operation == OPERATION_MUL)
					{
						calculation += OPERATOR_MUL_STRING_CALCULATION;
					}
					else
					if (operation == OPERATION_DIV)
					{
						calculation += OPERATOR_DIV_STRING_CALCULATION;
					}/*
					else
					if (operation == OPERATION_SQR)
					{
						calculation += OPERATOR_SQR_STRING_CALCULATION;
					}*/
					calculation += Common.BLANK_SPACE;
				}
				countForEach++;
			}
			newOperation = new Record() { Operation = operation, Calculation = calculation, Date = new System.DateTime() };
			getAllDataOperations().Add(newOperation);
		}
		public static List<Record> getQueryById(int idQuery)
		{
			if (idQuery >= FIRST_ID && idQuery <= getLastId())
			{
				return new List<Record>() { getAllDataOperations()[idQuery] };
			}
			else
			if (idQuery == ID_ALL_IDS)
			{
				return getAllDataOperations();
			}
			else
			{
				return null;
			}
		}
	}
}