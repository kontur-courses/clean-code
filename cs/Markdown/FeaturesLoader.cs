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
			AddToken(new BoldText());
			AddToken(new Link());
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