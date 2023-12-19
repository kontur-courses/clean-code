namespace Markdown.Token;

public class HeaderToken : IToken
{
    private const string TokenSeparator = "#";
    private const bool HasPair = false;

    public int Length => TokenSeparator.Length;
    public int Position { get; }
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    
    public HeaderToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source)
    {
        return Position == 0 || source[Position - 1] == '\n';
    }
}