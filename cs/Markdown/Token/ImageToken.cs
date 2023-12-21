namespace Markdown.Token;

public class ImageToken : IToken
{
    private const string TokenSeparator = "@";
    private const bool HasPair = true;

    public int Position { get; }
    public int EndingPosition { get; }
    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public bool IsParametrized => true;
    public string Parameters { get; set; }
    public int Shift { get; set; }

    public ImageToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, ref List<IToken> tokens, IToken currentToken)
    {
        if (!IsClosed)
            return true;

        if (source.Substring(currentToken.Position, Position - currentToken.Position).Contains(' '))
        {
            Parameters = source.Substring(currentToken.Position + 1, Position - currentToken.Position - 1);
            currentToken.Parameters = Parameters;
            currentToken.Shift = Parameters.Length;
            return true;
        }

        return false;
    }
}