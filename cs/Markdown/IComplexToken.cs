namespace Markdown
{
	internal interface IComplexToken: IToken
	{
		IComplexTokenBlock[] ChildTokens { get; }
		bool IsOpeningSequenceForChild(TokenizerContextState contextState, out IComplexTokenBlock childToken);
		void RepresentAsPlainText(TokenInfo tokenInfo, string sourceText);
	}
}