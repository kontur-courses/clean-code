namespace Markdown.Tokens.CommonTokens;

public class EscapeToken : CommonToken
{
    public override TokenType Type => TokenType.Escape;

    public override string Content => @"\";
}