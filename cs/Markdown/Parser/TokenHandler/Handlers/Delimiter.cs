using Markdown.Token;

namespace Markdown.Parser.TokenHandler;

public class Delimiter
{
    public string Opening { get; }
    public string Closing { get; }
    public TokenType Type { get; }
    
    public Delimiter(string opening, string closing, TokenType type)
    {
        Opening = opening;
        Closing = closing;
        Type = type;
    }
}