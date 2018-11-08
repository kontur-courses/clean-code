using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	class TokenReader
	{
		public Token ReadUntil(Func<char, bool> isStopChar)
		{
			throw new NotImplementedException();
		}

		public Token ReadWhile(Func<char, bool> accept)
		{
			throw new NotImplementedException();
		}

		int Position { get; }
	}
}
