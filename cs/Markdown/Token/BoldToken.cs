namespace Markdown.Token;

public class BoldToken : IToken
{
    private const string TokenSeparator = "__";
    private const bool HasPair = true;

    public int Length => TokenSeparator.Length;
    public int Position { get; }
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    
    public BoldToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source)
    {
        if (IsClosed && IsValidClose(source))
        {
            return true;
        }

        if (!IsClosed && IsValidOpen(source))
        {
            return true;
        }

        return false;
    }

    public bool IsValidOpen(string source)
    {
        return Position == 0 || (source.Length - 1 >= Position + Length && source[Position + Length] != ' ' &&
                                 !char.IsDigit(source[Position - 1]));
    }

    public bool IsValidClose(string source)
    {
        return !(source[Position - 1] == ' ' ||
                 source.Length - 1 >= Position + Length && char.IsDigit(source[Position + Length]));
    }
}