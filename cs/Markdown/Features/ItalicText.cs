namespace Markdown.Features
{
	internal class ItalicText: IToken
	{
		public string OpeningSequence { get; } = "_";
		public string ClosingSequence { get; } = "_";
		public bool PlainTextContent { get; }

		public bool IsOpeningKeySequence(TokenizerContextState contextState)
		{
			if (contextState.CurrentIndex + 1 >= contextState.SourceText.Length) return false;
			var nextChar = contextState.SourceText[contextState.CurrentIndex + 1].ToString();
			return contextState.CurrentKeySequence.ToString() == OpeningSequence &&
			       !string.IsNullOrWhiteSpace(nextChar) && 
			       nextChar != "_" && 
			       !int.TryParse(nextChar, out _);
		}

		public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo)
		{
			if (contextState.CurrentIndex - 1 < 0) return false;
			var previousChar = contextState.SourceText[contextState.CurrentIndex - 1].ToString();
			return !string.IsNullOrWhiteSpace(previousChar) &&
			       !int.TryParse(previousChar, out _) &&
			       contextState.CurrentKeySequence.ToString() == ClosingSequence &&
			       tokenInfo.InnerTokens.Count > 0;
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "<em>";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</em>";
	}
}