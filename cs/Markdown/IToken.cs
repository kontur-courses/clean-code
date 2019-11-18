namespace Markdown
{
	internal interface IToken
	{
		string OpeningSequence { get; }
		string ClosingSequence { get; }
		bool PlainTextContent { get; }
		bool IsOpeningKeySequence(TokenizerContextState contextState);
		bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo);
		string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText);
		string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText);
	}
}