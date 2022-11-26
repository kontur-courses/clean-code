using Markdown.Enums;

namespace Markdown.Tokens;

public class Token
{
    public Token(int start, int end, TokenType type)
    {
        Start = start;
        End = end;
        Type = type;
    }

    public int Start { get; set; }
    public int End { get; set; }
    public TokenType Type { get; set; }
}