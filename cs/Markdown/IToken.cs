namespace Markdown
{
	internal interface IToken
	{
		string OpeningSequence { get; }
		string ClosingSequence { get; }
		KeySequenceType RecognizeKeySequence(Context context, string sourceText);
		string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText);
		string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText);
	}
}