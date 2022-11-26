using Markdown.Enums;

namespace Markdown.Tokens;

public class Text : Token
{
    public Text(int start, int end, TokenType type, string value) : base(start, end, type)
    {
        Value = value;
    }

    public string Value { get; set; }
}