namespace Markdown.Tokens.Types;

public class TextToken : ITokenType
{
    public string Value { get; }

    public TextToken(string value)
    {
        Value = value;
    }

    public string Representation(bool isClosingTag) => Value;
}