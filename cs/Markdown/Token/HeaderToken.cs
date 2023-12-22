namespace Markdown.Token;

public class HeaderToken : IToken
{
    private const string TokenSeparator = "# ";
    private const bool HasPair = false;

    public int Length => IsClosed ? 1 : 2;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;

    public int Position { get; }
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int Shift { get; set; }

    public HeaderToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, ref List<IToken> tokens, IToken currentToken)
    {
        if (Position == 0 || source[Position - 1] == '\n')
        {
            var endingPosition = source.IndexOf('\n', Position);
            if (endingPosition < 0)
                endingPosition = source.Length;
            tokens.Add(new HeaderToken(endingPosition, true));
            return true;
        }

        return false;
    }
}