namespace Markdown.Features
{
	internal class PlainText: IToken
	{
		public string OpeningSequence { get; } = "";
		public string ClosingSequence { get; } = "";

		public bool IsOpeningKeySequence(TokenizerContextState contextState) => true;

		public bool IsClosingKeySequence(TokenizerContextState contextState, TokenInfo tokenInfo) => true;

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "";
	}
}