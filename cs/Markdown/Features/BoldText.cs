namespace Markdown.Features
{
	internal class BoldText: IToken
	{
		public string OpeningSequence { get; }
		public string ClosingSequence { get; }
		
		public KeySequenceType RecognizeKeySequence(TokenizerContextState context)
		{
			throw new System.NotImplementedException();
		}

		public string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText) => "<strong>";

		public string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText) => "</strong>";
	}
}