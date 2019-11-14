namespace Markdown
{
	internal interface IToken
	{
		string OpeningSequence { get; }
		string ClosingSequence { get; }
		KeySequenceType RecognizeKeySequence(TokenizerContextState context);
		string GetHtmlOpeningTag(TokenInfo tokenInfo, string sourceText);
		string GetHtmlClosingTag(TokenInfo tokenInfo, string sourceText);
	}
}