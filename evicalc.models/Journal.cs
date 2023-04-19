
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

		private static void _logStrJournalFile(NLog.Logger logger = null)
		{
			// Because it doesn't log the name of a model file
			Common.LogStr(Environment.NewLine+"Journal File", logger);
		}

		public static IList<int> GetListAllIdsOperations(bool idAllIdsIncluded = false, string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				var result = new List<int>();
				if (idAllIdsIncluded)
					result.Add(ID_ALL_IDS);

				for (int id = FIRST_ID; id <= GetLastID(fileSource, logger); id += RANGE_IDS)
					result.Add(id);

				Common.LogStr($"The all Ids Operations are {ParseListIntToString(result,logger)}" + Environment.NewLine, logger);
				return result;
			}
			Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			return null;
		}

		public static IList<Record> GetListDataOperationByIdList(int idQuery=ID_ALL_IDS, string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);

			Common.LogStr($"Searching the Id {idQuery} in the journal file and return the data in an Ilist<Record>", logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				var lines = File.ReadLines(fileSource);
				var result = new List<Record>();
				var lastRecord = new Record();
				var lastIdChecked = FIRST_ID;

				if (idQuery == ID_ALL_IDS)
				{
					Common.LogStr("Id All Ids", logger);
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
					Common.LogStr($"The result from all Ids is: {ParseListRecordToString(result, logger)}" + Environment.NewLine, logger);
					return result;
				}
				else if (!lines.Contains(ID_RECORD_PREFIX + idQuery))
				{
					Common.LogStr("Error, the id is not found in the journal file, so I return null" + Environment.NewLine, logger);
					return null;
				}

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
				Common.LogStr($"The result from Id {idQuery} is: {ParseListRecordToString(result, logger)}" + Environment.NewLine, logger);
				return result;
			}
			Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			return null;
		}

		public static string GetListDataOperationByIdString(int idQuery = ID_ALL_IDS, string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				var lines = File.ReadLines(fileSource);

				if (idQuery == ID_ALL_IDS)
				{
					Common.LogStr($"All Ids selected so I return: {String.Join(Environment.NewLine, lines)}" + Environment.NewLine, logger);
					return String.Join(Environment.NewLine, lines); // return all log content separate by lines
				}
				else if (!lines.Contains(ID_RECORD_PREFIX + idQuery))
				{
					Common.LogStr("Error, id not found in the journal file so I return null" + Environment.NewLine, logger);
					return null;
				}


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
				Common.LogStr($"Id found so I return the data from the Id in the journal file which is: {result}" + Environment.NewLine, logger);
				return result;
			}
			Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			return null;
		}

		public static string ParseListRecordToString(IList<Record> list = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (list != null)
			{
				Common.LogStr("Parsing List<Record> to string" + Environment.NewLine, logger);
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
			Common.LogStr("The list is null, so I return null" + Environment.NewLine, logger);
			return null;
		}
		public static string ParseListIntToString(IList<int> list = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (list != null)
			{
				Common.LogStr("Parsing List<Record> to string" + Environment.NewLine, logger);
				var result = "";
				foreach (var item in list)
				{
					result += item + Environment.NewLine;
				}
				return result;
			}
			Common.LogStr("The list is null, so I return null" + Environment.NewLine, logger);
			return null;
		}


		public static void AddDataOperation(char operation=' ', string calculation="", DateTime? date = null, string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				if (date == null)
				{
					Common.LogStr("The date is null, so I convert it to DateTime.UtcNow", logger);
					date = DateTime.UtcNow;
				}

				Common.LogStr("Add the data in _operations" + Environment.NewLine, logger);
				_operations.Add(new Record() { Id = _currentId, Operation = operation, Calculation = calculation, Date = date.Value });
				WriteJournalOperationFile(operation, calculation, date, fileSource, logger);
			}
			else
			{
				Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			}
		}

		public static void WriteJournalOperationFile(char operation = ' ', string calculation = "", DateTime? date = null, string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				var sb = new StringBuilder();
				var operationData = OPERATION_RECORD_PREFIX + operation.ToString();
				var calculationData = CALCULATION_RECORD_PREFIX + calculation.ToString();
				var dateData = DATE_RECORD_PREFIX + date.ToString();
				sb.AppendLine(operationData);
				sb.AppendLine(calculationData);
				sb.AppendLine(dateData);
				sb.AppendLine(FINAL_DATA_OPERATION);
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
				Common.LogStr($"Writing An Operation Journal to the Journal File. The values are: {operationData} | {calculationData} | {dateData} | {FINAL_DATA_OPERATION}" + Environment.NewLine, logger);
			}
			else
			{
				Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			}
		}

		public static int? GetLastID(string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				if (_lastId != null)
				{
					Common.LogStr($"_lastId is not null, so I return it, which value is: {_lastId}" + Environment.NewLine, logger);
					return _lastId.Value;
				}
				Common.LogStr("_lastId is null, so I'll search it on the journal file", logger);
				var lines = File.ReadLines(fileSource);
				if (!lines.Contains(ID_RECORD_PREFIX + FIRST_ID))
				{
					Common.LogStr("The Journal file doesn't have the first possible ID, it should be empty or corrupted so I return null" + Environment.NewLine, logger);
					return null;
				}

				foreach (var line in lines)
				{
					if (line.Contains(ID_RECORD_PREFIX))
						_lastId = int.Parse(line.Substring(CHAR_DATA_ID_FILE));
				}

				Common.LogStr($"I find the last ID in the journal file so I assign it to _lastId and return it, the last id is: {_lastId}" + Environment.NewLine, logger);
				return _lastId.Value;
			}
			Common.LogStr("Error, the file source is null, so I return null" + Environment.NewLine, logger);
			return null;
		}

		public static void LogNewId(string fileSource = null, NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			if (fileSource != null)
			{
				Common.CheckFileExist(fileSource, true, logger);
				var sb = new StringBuilder();
				if (GetLastID(fileSource, logger) == null)
				{
					sb.AppendLine(ID_RECORD_PREFIX + FIRST_ID);
					_lastId = FIRST_ID;
					_currentId = FIRST_ID;
				}
				else
				{
					var newId = GetLastID(fileSource, logger) + RANGE_IDS;
					sb.AppendLine(ID_RECORD_PREFIX + newId);
					_lastId = newId;
					_currentId = newId.Value;
				}
				File.AppendAllText(fileSource, sb.ToString());
				sb.Clear();
				Common.LogStr($"New ID: {_currentId}" + Environment.NewLine, logger);
			}
		}

		public static int GetCurrentID(NLog.Logger logger = null)
		{
			_logStrJournalFile(logger);
			Common.PrintCurrentMethod(logger);
			var resultInValue = _currentId; // The Idea is not allow to modify outside, but get the value and modify the value in this class only
			Common.LogStr($"Current ID is: {resultInValue}" + Environment.NewLine, logger);
			return resultInValue;
		}



	}
}