namespace Markdown.Tokens.HtmlToken;

public class TextToken : SingleToken
{
    public TextToken(string value) : base(value)
    {
    }

    public override string ToString()
    {
        return Value;
    }
}