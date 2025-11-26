namespace Markdown;

public class Token
{
    public string Value { get; }
    public TokenType Type { get; }
    public int Position { get; }


    public Token(TokenType type, string value, int position)
    {
        Value = value;
        Type = type;
        Position = position;

    }


    
    public override bool Equals(object? obj)
    {
        if (obj is Token other)
        {
            return Value == other.Value && 
                   Type == other.Type && 
                   Position == other.Position;
        }
        return false;
    }
    
    public override string ToString()
    {
        return $"[{Type} \"{Value}\" pos={Position}]";
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Type, Position);
    }

}