namespace Markdown.Token;

public class ItalicToken : IToken
{
    private const string TokenSeparator = "_";
    private const bool HasPair = true;

    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public int Position { get; }
    public int EndingPosition { get; private set; }
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public string Parameters { get; set; }
    public int Shift { get; set; }

    public ItalicToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, ref List<IToken> tokens, IToken currentToken)
    {
        return (IsClosed && this.IsValidClose(source)) || (!IsClosed && this.IsValidOpen(source));
    }
}