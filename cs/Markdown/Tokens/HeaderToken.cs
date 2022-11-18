namespace Markdown.Tokens;

public class HeaderToken : TextContainerToken
{
    public HeaderToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.Header;
}