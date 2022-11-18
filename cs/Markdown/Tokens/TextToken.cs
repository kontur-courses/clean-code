namespace Markdown.Tokens;

public class TextToken : TextContainedToken
{
    public TextToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.Plain;

    public override void AddChildren(Token child)
    {
        throw new ApplicationException($"Cannot add token {child}");
    }
}