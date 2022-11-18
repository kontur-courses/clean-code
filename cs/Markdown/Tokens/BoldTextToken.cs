namespace Markdown.Tokens;

public class BoldTextToken : TextContainedToken
{
    public BoldTextToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.Bold;
}