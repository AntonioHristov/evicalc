﻿using System.Collections;
using System.Collections.Generic;

namespace evicalc.models
{
	public class AddRequest
	{
		public IList<double> Addens { get; set; } = new List<double>();
	}
}