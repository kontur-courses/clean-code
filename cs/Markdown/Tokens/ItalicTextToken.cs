namespace Markdown.Tokens;

public class ItalicTextToken : TextToken
{
    public ItalicTextToken(Token parent, string value) : base(parent, value)
    {
    }

    public override TokenType Type => TokenType.Italic;

    public Range Range { get; set; }

    public override void AddChildren(Token child)
    {
        throw new ApplicationException($"Cannot add token {child}");
    }
}