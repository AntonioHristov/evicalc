using System.Collections;

namespace evicalc.models
{
	public class Common
	{
		public const string STRING_EMPTY = "";
		public const string BLANK_SPACE = " ";
		public const int FIRST_POSITION_ARRAY = 0;
		public const int LENGHT_LIST_EMPTY = 0;

		public static string getStringManyTimes(string text="", int times=1 )
		{
			var result = "";
			for(int count = 0; count<times; count++)
			{
				result += text;
			}
			return result;
		}

		public static int getLenghtList(IList list)
		{
			return list.Count;
		}

		public static int getLastPositionList(IList list)
		{
			return getLenghtList(list) - 1;
		}



		public static int getLenghtIEnumerable(IEnumerable ienumerable)
		{
			var count = 0;
			foreach (var item in ienumerable)
			{
				count++;
			}
			return count;
		}

		public static int getLastPositionIEnumerable(IEnumerable ienumerable)
		{
			return getLenghtIEnumerable(ienumerable)-1;
		}

	}
}
