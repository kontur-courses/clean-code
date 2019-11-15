using System;
using System.Collections.Generic;
using Markdown.Features;

namespace Markdown
{
	internal class FeaturesLoader
	{
		public List<IToken> SupportedTokens { get; } = new List<IToken>();

		public FeaturesLoader()
		{
			AddToken(new ItalicText());
		}

		private void AddToken(IToken newToken)
		{
			if (string.IsNullOrEmpty(newToken.OpeningSequence) ||
			    string.IsNullOrEmpty(newToken.ClosingSequence))
				throw new ArgumentException("Key sequence can't be null or empty");
			
			SupportedTokens.Add(newToken);
		}
	}
}