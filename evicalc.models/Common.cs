using System.Collections;

namespace evicalc.models
{
	public class Common
	{
		public const string STRING_EMPTY = "";
		public const string BLANK_SPACE = " ";
		public const int FIRST_POSITION_ARRAY = 0;
		public const int LENGHT_LIST_EMPTY = 0;

		public static string GetStringManyTimes(string text="", int times=1 )
		{
			var result = "";
			for(int count = 0; count<times; count++)
			{
				result += text;
			}
			return result;
		}

		public static int GetLastPositionList(IList list)
		{
			return list.Count - 1;
		}

		// Although I use "using System.Linq;", "ienumerable.Count();" doesn't work
		public static int GetLenghtIEnumerable(IEnumerable ienumerable)
		{
			var count = 0;
			foreach (var item in ienumerable)
			{
				count++;
			}
			return count;
		}

		public static int GetLastPositionIEnumerable(IEnumerable ienumerable)
		{
			return GetLenghtIEnumerable(ienumerable)-1;
		}

	}
}
