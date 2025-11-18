namespace Markdown.Tokens;

internal class TextToken : IToken
{
    public string Value { get; }
    public int Length => Value.Length;

    public TextToken(string text) => Value = text;
}
