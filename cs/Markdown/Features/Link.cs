namespace Markdown.Features
{
	internal class Link: IToken
	{
		public string OpeningSequence { get; }
		public string ClosingSequence { get; }
		
		public KeySequenceType RecognizeKeySequence(TokenizerContextState context)
		{
			throw new System.NotImplementedException();
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText)
		{
			var link = ExtractLink(tokenInfo, sourceText);
			return $"<a href='{link}'>";
		}

		private static string ExtractLink(TokenInfo tokenInfo, string sourceText)
		{
			throw new System.NotImplementedException();
		}

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</a>";
	}
}