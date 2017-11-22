using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class RawToken
	{
		public string Type { get; }
		public string Value { get; }

		public RawToken(string type, string value=null)
		{
			Type = type;
			Value = value;
		}

	}
}
