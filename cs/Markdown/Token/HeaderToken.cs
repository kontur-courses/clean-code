using System.Runtime.CompilerServices;

namespace Markdown.Token;

public class HeaderToken : IToken
{
    private const string TokenSeparator = "#";
    private const bool HasPair = false;

    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;

    public int Position { get; }
    public bool IsClosed { get; set; }
    public int EndingPosition { get; private set; }

    public HeaderToken(int position, bool isClosed = false)
    {
        Position = position;
        IsClosed = isClosed;
    }

    public bool IsValid(string source, ref List<IToken> tokens)
    {
        if (Position == 0 || source[Position - 1] == '\n')
        {
            EndingPosition = source.IndexOf('\n', Position);
            if (EndingPosition < 0)
                EndingPosition = source.Length;
            tokens.Add(new HeaderToken(EndingPosition, true));
            return true;
        }

        return false;
    }
}