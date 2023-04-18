
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
		public const int RANGE_IDS = 1;
		public const int CHAR_DATA_ID_FILE = 4;
		public const int CHAR_DATA_OPERATION_FILE = 11;
		public const int CHAR_DATA_CALCULATION_FILE = 13;
		public const int CHAR_DATA_DATE_FILE = 6;
		public const string ID_RECORD_PREFIX = "Id: ";
		public const string OPERATION_RECORD_PREFIX = "Operation: ";
		public const string CALCULATION_RECORD_PREFIX = "Calculation: ";
		public const string DATE_RECORD_PREFIX = "Date: ";
		public const string FINAL_DATA_OPERATION = "";
		public static readonly DateTime? _dateNow = null;
		public static IList<Record> _operations = new List<Record>() { };
		private static int _currentId = FIRST_ID; // If another user is connected after, it will be not lastId. Atleast this is the idea
		private static int? _lastId = null;

		public static IList<int> GetListAllIdsOperations(bool idAllIdsIncluded = false, string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				var result = new List<int>();
				if (idAllIdsIncluded)
					result.Add(ID_ALL_IDS);

				for (int id = FIRST_ID; id <= GetLastID(fileSource); id += RANGE_IDS)
					result.Add(id);

				return result;
			}
			return null;
		}

		public static IList<Record> GetListDataOperationByIdList(int idQuery=ID_ALL_IDS, string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				var lines = File.ReadLines(fileSource);
				var result = new List<Record>();
				var lastRecord = new Record();
				var lastIdChecked = FIRST_ID;

				if (idQuery == ID_ALL_IDS)
				{
					foreach (var line in lines)
					{
						if (line.Contains(ID_RECORD_PREFIX))
						{
							lastRecord.Id = int.Parse(line.Substring(CHAR_DATA_ID_FILE));
							lastIdChecked = lastRecord.Id;
						}
						else if (line.Contains(OPERATION_RECORD_PREFIX))
							lastRecord.Operation = line[CHAR_DATA_OPERATION_FILE];
						else if (line.Contains(CALCULATION_RECORD_PREFIX))
							lastRecord.Calculation = line.Substring(CHAR_DATA_CALCULATION_FILE);
						else if (line.Contains(DATE_RECORD_PREFIX))
							lastRecord.Date = DateTime.Parse(line.Substring(CHAR_DATA_DATE_FILE));
						else if (line == FINAL_DATA_OPERATION)
						{
							if (lastRecord.Id == FIRST_ID)
								lastRecord.Id = lastIdChecked;
							result.Add(lastRecord);
							lastRecord = new Record();
						}
					}
					return result;
				}
				else if (!lines.Contains(ID_RECORD_PREFIX + idQuery))
					return null;

				var dataFromId = false;
				foreach (var line in lines)
				{
					if (line.Contains(ID_RECORD_PREFIX + idQuery))
						dataFromId = true;
					else if (line == ID_RECORD_PREFIX + (idQuery + RANGE_IDS))
						dataFromId = false;

					if (dataFromId)
					{
						if (line.Contains(ID_RECORD_PREFIX))
						{
							lastRecord.Id = int.Parse(line.Substring(CHAR_DATA_ID_FILE));
							lastIdChecked = lastRecord.Id;
						}
						else if (line.Contains(OPERATION_RECORD_PREFIX))
							lastRecord.Operation = line[CHAR_DATA_OPERATION_FILE];
						else if (line.Contains(CALCULATION_RECORD_PREFIX))
							lastRecord.Calculation = line.Substring(CHAR_DATA_CALCULATION_FILE);
						else if (line.Contains(DATE_RECORD_PREFIX))
							lastRecord.Date = DateTime.Parse(line.Substring(CHAR_DATA_DATE_FILE));
						else if (line == FINAL_DATA_OPERATION)
						{
							if (lastRecord.Id == FIRST_ID)
								lastRecord.Id = lastIdChecked;
							result.Add(lastRecord);
							lastRecord = new Record();
						}
					}
				}
				return result;
			}
			return null;
		}

		public static string GetListDataOperationByIdString(int idQuery = ID_ALL_IDS, string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				var lines = File.ReadLines(fileSource);

				if (idQuery == ID_ALL_IDS)
					return String.Join(Environment.NewLine, lines); // return all log content separate by lines
				else if (!lines.Contains(ID_RECORD_PREFIX + idQuery))
					return null;

				var result = "";
				var dataFromId = false;
				foreach (var line in lines)
				{
					if (line.Contains(ID_RECORD_PREFIX + idQuery))
						dataFromId = true;
					else if (line.Contains(ID_RECORD_PREFIX + (idQuery + RANGE_IDS)))
						dataFromId = false;
					if (dataFromId)
						result += Environment.NewLine + line;
				}
				return result;
			}
			return null;
		}

		public static string ParseListRecordToString(IList<Record> list)
		{
			if (list != null)
			{
				var result = "";
				foreach (var item in list)
				{
					result += Environment.NewLine + ID_RECORD_PREFIX + item.Id + Environment.NewLine;
					result += OPERATION_RECORD_PREFIX + item.Operation + Environment.NewLine;
					result += CALCULATION_RECORD_PREFIX + item.Calculation + Environment.NewLine;
					result += DATE_RECORD_PREFIX + item.Date + Environment.NewLine;
				}
				return result;
			}
			return null;
		}

		public static void AddDataOperation(char operation=' ', string calculation="", DateTime? date = null, string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				if (date == null)
					date = DateTime.UtcNow;

				_operations.Add(new Record() { Id = _currentId, Operation = operation, Calculation = calculation, Date = date.Value });
				if (fileSource != null)
					CreateLog(operation, calculation, date, fileSource);
			}
		}

		public static void CreateLog(char operation = ' ', string calculation = "", DateTime? date = null, string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				var sb = new StringBuilder();
				sb.AppendLine(OPERATION_RECORD_PREFIX + operation.ToString());
				sb.AppendLine(CALCULATION_RECORD_PREFIX + calculation.ToString());
				sb.AppendLine(DATE_RECORD_PREFIX+ date.ToString());
				sb.AppendLine(FINAL_DATA_OPERATION);
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
			}
		}

		public static int? GetLastID(string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				if (_lastId != null)
					return _lastId.Value;
				var lines = File.ReadLines(fileSource);
				if (!lines.Contains(ID_RECORD_PREFIX + FIRST_ID))
					return null;

				foreach (var line in lines)
				{
					if (line.Contains(ID_RECORD_PREFIX))
						_lastId = int.Parse(line.Substring(CHAR_DATA_ID_FILE));
				}
				return _lastId.Value;
			}
			return null;
		}

		public static void LogNewId(string fileSource = null)
		{
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true);
				var sb = new StringBuilder();
				if (GetLastID(fileSource) == null)
				{
					sb.AppendLine(ID_RECORD_PREFIX + FIRST_ID);
					_lastId = FIRST_ID;
					_currentId = FIRST_ID;
				}
				else
				{
					var newId = GetLastID(fileSource) + RANGE_IDS;
					sb.AppendLine(ID_RECORD_PREFIX + newId);
					_lastId = newId;
					_currentId = newId.Value;
				}
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
			}
		}

		public static int GetCurrentID()
		{
			var resultInValue = _currentId; // The Idea is not allow to modify outside, but get the value and modify the value in this class only
			return resultInValue;
		}
	}
}