namespace Markdown.Tokens;

public class MdTextToken : IToken
{
    public string Value { get; }
    public int Length => Value.Length;

    public MdTextToken(string text)
    {
        Value = text;
    }
}