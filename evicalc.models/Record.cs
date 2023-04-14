using System;

namespace evicalc.models
{
	public class Record
	{
		public char Operation { get; set; }
		public string Calculation { get; set; }
		public DateTime Date { get; set; }
	}
}