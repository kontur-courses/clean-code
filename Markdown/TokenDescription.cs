using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	public class TokenDescription
	{
		public string Type { get; }
		public string pattern { get; }
		public Func<char?, char?, TagType> TagTypeDeterminant { get; }
		public string OpeningTag { get; }

		public string ClosingTag
		{
			get
			{
				return new StringBuilder(OpeningTag).Insert(1, "/").ToString();
			}
		}

		public TokenDescription(string type, string pattern, string openingTag, Func<char?, char?, TagType> tagTypeDeterminant)
		{
			Type = type;
			this.pattern = pattern;
			OpeningTag = openingTag;
			TagTypeDeterminant = tagTypeDeterminant;
		}

	}
}
