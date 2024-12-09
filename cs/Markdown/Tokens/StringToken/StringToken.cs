namespace Markdown.Tokens.StringToken;

public class StringToken(string value, int offset, StringTokenType type)
{
    public int Length = value.Length;
    public int Offset = offset;
    public string Value = value;
    public StringTokenType Type = type;
}