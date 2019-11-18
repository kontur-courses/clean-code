namespace Markdown
{
	internal interface IComplexTokenBlock: IToken
	{
		IComplexToken Parent { get; }
	}
}