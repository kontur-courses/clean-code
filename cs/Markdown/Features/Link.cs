using System.Linq;

namespace Markdown.Features
{
	internal class Link: IComplexToken
	{
		public string OpeningSequence { get; } = "[";
		public string ClosingSequence { get; } = ")";
		public IComplexTokenBlock[] ChildTokens { get; }
		public bool PlainTextContent { get; }

		public Link() => 
			ChildTokens = new IComplexTokenBlock[] {new LinkText(this), new LinkReference(this), };

		public bool IsOpeningSequenceForChild(TokenizerContextState contextState, out IComplexTokenBlock token)
		{
			token = ChildTokens[0];
			return contextState.CurrentChar.ToString() == ChildTokens[0].OpeningSequence;
		}

		public void RepresentAsPlainText(TokenInfo tokenInfo, string sourceText)
		{
			tokenInfo.InnerTokens.Insert(0, tokenInfo.Blocks[0]);
			if (tokenInfo.Blocks[0].Closed)
			{
				var closingChar = new TokenInfo(tokenInfo.Blocks[0].EndIndex, new PlainText());
				closingChar.PlainText.Append("]");
				tokenInfo.InnerTokens.Insert(1, closingChar);
			}
			if (tokenInfo.Blocks.Count == 2)
				tokenInfo.InnerTokens.Insert(2, tokenInfo.Blocks[1]);
		}

		public bool IsOpeningKeySequence(TokenizerContextState contextState) => 
			contextState.CurrentKeySequence.ToString() == OpeningSequence;

		public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo)
		{
			return tokenInfo.Blocks.Count == 2 &&
			       tokenInfo.Blocks[0].EndIndex == tokenInfo.Blocks[1].StartIndex - 1 &&
			       tokenInfo.Blocks[0].Closed &&
			       !tokenInfo.Blocks[1].PlainText.ToString().Any(char.IsWhiteSpace) &&
				contextState.CurrentChar.ToString() == ClosingSequence;
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText)
		{
			var linkReference = tokenInfo.Blocks[1].PlainText;
			tokenInfo.InnerTokens.AddRange(tokenInfo.Blocks[0].InnerTokens);
			return $"<a href='{linkReference}'>";
		}

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</a>";


		private class LinkText: IComplexTokenBlock
		{
			public string OpeningSequence { get; } = "[";
			public string ClosingSequence { get; } = "]";
			public bool PlainTextContent { get; }
			public IComplexToken Parent { get; }

			public LinkText(IComplexToken parent) => Parent = parent;

			public bool IsOpeningKeySequence(TokenizerContextState contextState) => 
				contextState.CurrentKeySequence.ToString() == OpeningSequence;

			public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo) => 
				contextState.CurrentKeySequence.ToString() == ClosingSequence;

			public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "";

			public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "";
		}

		private class LinkReference: IComplexTokenBlock
		{
			public string OpeningSequence { get; } = "(";
			public string ClosingSequence { get; } = ")";
			public bool PlainTextContent { get; } = true;
			public IComplexToken Parent { get; }
			
			public LinkReference(IComplexToken parent) => Parent = parent;
			
			public bool IsOpeningKeySequence(TokenizerContextState contextState)
			{
				return contextState.CurrentKeySequence.ToString() == OpeningSequence &&
					contextState.CurrentIndex - 1 >= 0 && 
					contextState.SourceText[contextState.CurrentIndex - 1] == ']';
			}

			public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo) =>
				contextState.CurrentKeySequence.ToString() == ClosingSequence;

			public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "";

			public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "";
		}
	}
}