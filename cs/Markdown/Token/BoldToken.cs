namespace Markdown.Token;

public class BoldToken : IToken
{
    private const string TokenSeparator = "__";
    private const bool HasPair = true;

    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public int Position { get; }
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    public BoldToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        return (IsClosed && this.IsValidClose(source)) || (!IsClosed && this.IsValidOpen(source));
    }
}