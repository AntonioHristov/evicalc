
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace evicalc.models
{
	public static class Journal
	{
		public const int ID_ALL_IDS = -1;
		public const int FIRST_ID = Common.FIRST_POSITION_ARRAY; // 0
		public static readonly DateTime? _dateNow = null;
		private static int? _lastId = null;
		public static List<Record> _operations = new List<Record>() { };

		public static int GetLastId() => _operations.Count - 1;

		public static List<int> GetListAllIdsOperations(bool idAllIdsIncluded = false, string fileSource = null)
		{
			var result = new List<int>();
			if (idAllIdsIncluded)
				result.Add(ID_ALL_IDS);

			for (int id = FIRST_ID; id <= GetLastID(fileSource); id++)
				result.Add(id);

			return result;
		}
		/*
		public static List<Record> GetListDataOperationById(int idQuery)
		{
			if (idQuery >= FIRST_ID && idQuery <= GetLastId())
				return new List<Record>() { _operations[idQuery] };
			else
			if (idQuery == ID_ALL_IDS)
				return _operations;

			return null;
		}
		*/
		public static string GetListDataOperationById(int idQuery = ID_ALL_IDS, string fileSource = null)
		{
			if (fileSource != null)
			{
				var lines = File.ReadLines(fileSource);

				if (idQuery == ID_ALL_IDS)
				{
					return String.Join(Environment.NewLine, lines); // return all log content separate by lines
				}
				else if (!lines.Contains($"ID: {idQuery}"))
					return null;


				var result = "";
				var idEncontrado = false;
				foreach (var line in lines)
				{
					if (line.Contains($"ID: {idQuery}"))
					{
						idEncontrado = true;
					}
					if (line.Contains($"ID: {idQuery+1}"))
					{
						idEncontrado = false;
					}
					if (idEncontrado)
					{
						result += Environment.NewLine + line;
					}
				}
				return result;
			}
			return null;
		}

			public static void AddDataOperation(char operation=' ', string calculation="", DateTime? date = null, string fileSource = null)
		{
			if (date == null)
				date = DateTime.UtcNow;

			_operations.Add(new Record() { Operation = operation, Calculation = calculation, Date = date.Value });
			if(fileSource != null)
				CreateLog(operation,calculation,date, fileSource);
		}

		public static void CreateLog(char operation = ' ', string calculation = "", DateTime? date = null, string fileSource = null)
		{
			if (fileSource != null)
			{
				var sb = new StringBuilder();
				sb.AppendLine(operation.ToString());
				sb.AppendLine(calculation.ToString());
				sb.AppendLine(date.ToString());
				sb.AppendLine("");
				//File.AppendAllText("./../log.txt", sb.ToString());
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
			}
		}

		public static int? GetLastID(string fileSource = null)
		{
			if (fileSource != null)
			{
				if (_lastId != null)
					return _lastId.Value;
				//string fileSource = "./../../../log.txt";
				var lines = File.ReadLines(fileSource);
				if (!lines.Contains($"ID: {FIRST_ID}"))
				{
					return null;
				}

				foreach (var line in lines)
				{
					if (line.Contains("ID: "))
					{
						_lastId = int.Parse(line.Substring(4));
					}
				}
				return _lastId.Value;
			}
			return null;
		}

		public static void LogNewId(string fileSource = null)
		{
			//string fileSource = "./../../../log.txt";
			if (fileSource != null)
			{
				var sb = new StringBuilder();
				if (GetLastID(fileSource) == null)
				{
					sb.AppendLine($"ID: {FIRST_ID}");
					_lastId = FIRST_ID;
				}
				else
				{
					var newId = GetLastID(fileSource) + 1;
					sb.AppendLine($"ID: {newId}");
					_lastId = newId;
				}
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
			}
		}


			// FIXME: Move it and may refactor (to api).
	}
}