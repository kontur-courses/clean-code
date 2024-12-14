namespace Markdown.Tokens.CommonTokens;

public class NewLineToken : CommonToken
{
    public override TokenType Type => TokenType.NewLine;

    public override string Content => Environment.NewLine;
}