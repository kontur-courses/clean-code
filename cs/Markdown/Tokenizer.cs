using System;
using System.Collections.Generic;

namespace Markdown
{
	internal class Tokenizer
	{
		public Dictionary<string, IToken> AvailableKeySequences { get; }

		public Tokenizer(Dictionary<string, IToken> availableKeySequences)
		{
			AvailableKeySequences = availableKeySequences;
		}
		
		public TokenInfo ParseToTokens(string sourceText)
		{
			var contextState = new TokenizerContextState(sourceText);
			throw new NotImplementedException();
		}
	}
}