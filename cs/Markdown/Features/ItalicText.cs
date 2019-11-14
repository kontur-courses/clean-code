namespace Markdown.Features
{
	internal class ItalicText: IToken
	{
		public string OpeningSequence { get; }
		public string ClosingSequence { get; }
		
		public KeySequenceType RecognizeKeySequence(TokenizerContextState context)
		{
			throw new System.NotImplementedException();
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "<em>";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</em>";
	}
}