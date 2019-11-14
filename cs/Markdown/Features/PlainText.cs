namespace Markdown.Features
{
	internal class PlainText: IToken
	{
		public string OpeningSequence { get; }
		public string ClosingSequence { get; }
		
		public KeySequenceType RecognizeKeySequence(TokenizerContextState context)
		{
			throw new System.NotImplementedException();
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "";
	}
}