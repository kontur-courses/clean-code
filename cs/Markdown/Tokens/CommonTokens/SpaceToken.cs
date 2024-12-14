namespace Markdown.Tokens.CommonTokens;

public class SpaceToken : CommonToken
{
    public override TokenType Type => TokenType.Space;

    public override string Content => " ";
}