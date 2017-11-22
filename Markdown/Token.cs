using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class Token
	{
		public string Type { get; }
		public TagType TagType { get; }
		public string Value { get; private set; }

		public Token(string type, TagType tagType, string value)
		{
			Type = type;
			TagType = tagType;
			Value = value;
		}

		public Token(string type, TagType tagType)
		{
			Type = type;
			TagType = tagType;
			Value = null;
		}

		public void AmplifyValue(string addition)
		{
			Value += addition;
		}

	}
}
