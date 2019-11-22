namespace Markdown.Features
{
	internal class BoldText: IToken
	{
		public string OpeningSequence { get; } = "__";
		public string ClosingSequence { get; } = "__";
		public bool PlainTextContent { get; }

		public bool IsOpeningKeySequence(TokenizerContextState contextState)
		{
			if (contextState.CurrentIndex + 1 >= contextState.SourceText.Length) return false;
			var nextChar = contextState.SourceText[contextState.CurrentIndex + 1].ToString();
			return contextState.CurrentKeySequence.ToString() == OpeningSequence &&
			       CheckIsInItalicToken(contextState) &&
			       !string.IsNullOrWhiteSpace(nextChar) && 
			       nextChar != "_" &&
			       !int.TryParse(nextChar, out _);
		}

		private static bool CheckIsInItalicToken(TokenizerContextState contextState)
		{
			var currentToken = contextState.TokenStack.LastToken;
			while (currentToken != null)
			{
				if (currentToken.TokenInfo.Type is ItalicText)
					return false;
				currentToken = currentToken.Parent;
			}
			return true;
		}

		public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo)
		{
			if (contextState.CurrentIndex - 2 < 0) return false;
			var previousChar = contextState.SourceText[contextState.CurrentIndex - 2].ToString();
			return !string.IsNullOrWhiteSpace(previousChar) &&
			       previousChar != "_" && 
			       !int.TryParse(previousChar, out _) &&
			       contextState.CurrentKeySequence.ToString() == ClosingSequence &&
			       tokenInfo.InnerTokens.Count > 0;
		}
		
		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "<strong>";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</strong>";
	}
}