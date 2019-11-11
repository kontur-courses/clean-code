namespace Markdown.Features
{
	internal class ItalicText: IToken
	{
		public string OpeningSequence { get; }
		public string ClosingSequence { get; }
		public bool IsComplex { get; }

		public string GetOpeningTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new System.NotImplementedException();
		}

		public string GetClosingTag(TokenInfo tokenInfo, string sourceText)
		{
			throw new System.NotImplementedException();
		}
	}
}