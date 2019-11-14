using System.Collections.Generic;
using Markdown.Features;

namespace Markdown
{
	internal class FeaturesLoader
	{
		public Dictionary<string, IToken> AvailableKeySequences = new Dictionary<string, IToken>();

		public FeaturesLoader()
		{
			AddToken(new PlainText());
		}

		private void AddToken(IToken token)
		{
			AvailableKeySequences.Add(token.OpeningSequence, token);
			AvailableKeySequences.Add(token.ClosingSequence, token);
		}
	}
}