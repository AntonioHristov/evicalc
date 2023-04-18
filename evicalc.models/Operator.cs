using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace evicalc.models
{
	public class Operator
	{
		public char _operatorAdd { get; set;}
		public char _operatorSub { get; set; }
		public char _operatorMul { get; set; }
		public char _operatorDiv { get; set; }
		public char _operatorSqr { get; set; }
		public char _operatorResult { get; set; }

		public Operator(char opAdd = ' ', char opSub = ' ', char opMul = ' ', char opDiv = ' ', char opSqr = ' ', char opResult = ' ')
		{
			this._operatorAdd = opAdd;
			this._operatorSub = opSub;
			this._operatorMul = opMul;
			this._operatorDiv = opDiv;
			this._operatorSqr = opSqr;
			this._operatorResult = opResult;
		}

		public IList<char> GetOperators(bool resultIncluded = false)
		{
			if(resultIncluded)
				return new List<char>() { this._operatorAdd, this._operatorSub, this._operatorMul, this._operatorDiv, this._operatorSqr, this._operatorResult };
			return new List<char>() { this._operatorAdd, this._operatorSub, this._operatorMul, this._operatorDiv, this._operatorSqr};
		}
	}
}