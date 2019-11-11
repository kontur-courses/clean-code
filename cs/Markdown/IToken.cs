namespace Markdown
{
	internal interface IToken
	{
		string OpeningSequence { get; }
		string ClosingSequence { get; }
		bool IsComplex { get; }
		string GetOpeningTag(TokenInfo tokenInfo, string sourceText);
		string GetClosingTag(TokenInfo tokenInfo, string sourceText);
	}
}