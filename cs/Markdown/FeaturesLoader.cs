using System.Collections.Generic;
using Markdown.Features;

namespace Markdown
{
	internal static class FeaturesLoader
	{
		public static Dictionary<string, IToken> AvailableKeySequences = new Dictionary<string, IToken>();

		public static void LoadFeatures()
		{
			AddToken(new Link());
		}

		private static void AddToken(IToken token)
		{
			AvailableKeySequences.Add($"{token.OpeningSequence}", token);
			AvailableKeySequences.Add($"{token.ClosingSequence}", token);
		}
	}
}