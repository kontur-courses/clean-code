using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	static class TokenReaderExtensions
	{
		public static Token SkipSpaces(this TokenReader reader)
		{
			return new Token();
		}
		public static Token ReadField(this TokenReader reader)
		{
			return new Token();
		}

		public static Token ReadSimpleField(this TokenReader reader)
		{
			return new Token();
		}

		public static Token ReadQuotedField(this TokenReader reader)
		{
			return new Token();
		}
	}
}
