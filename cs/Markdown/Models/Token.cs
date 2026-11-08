namespace Markdown.Models;

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Position { get; }
    
    public Token(TokenType type, string value, int position) 
    {   
        Type = type; 
        Value = value; 
        Position = position; 
    }
}