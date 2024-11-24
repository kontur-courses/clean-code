namespace Markdown.Tokens;

public class MdTextToken(string value) : IToken
{
    public string Value { get; } = value;
    public int Length => Value.Length;
    
}