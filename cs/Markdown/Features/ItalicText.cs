namespace Markdown.Features
{
	internal class ItalicText: IToken
	{
		public string OpeningSequence { get; } = "_";
		public string ClosingSequence { get; } = "_";

		public bool IsOpeningKeySequence(TokenizerContextState contextState)
		{
			var currentIndex = contextState.CurrentIndex;
			return currentIndex + 1 < contextState.SourceText.Length &&
			       !char.IsWhiteSpace(contextState.SourceText[currentIndex + 1]);
		}

		public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo)
		{
			return contextState.CurrentChar == ClosingSequence &&
				tokenInfo.InnerTokens.Count > 0;
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "<em>";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</em>";
	}
}