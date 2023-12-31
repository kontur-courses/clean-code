using System.ComponentModel.DataAnnotations;

namespace Markdown.Token;

public class HeaderToken : IToken
{
    private const string TokenSeparator = "# ";
    private const bool HasPair = false;
    private int endingLength = 1;

    public int Length => IsClosed ? endingLength : 2;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;

    public int Position { get; }
    public bool IsClosed { get; set; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    public HeaderToken(int position, bool isClosed = false, int endingLength = 1)
    {
        Position = position;
        IsClosed = isClosed;
        this.endingLength = endingLength;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        if (Position != 0 && source[Position - 1] != '\n')
            return false;
        var tagEndingLength = 1;
        var endingPosition = source.IndexOf('\n', Position);
        if (endingPosition > 0 && source[endingPosition - 1] == '\r')
        {
            endingPosition--;
            tagEndingLength = 2;
        }

        if (endingPosition < 0)
            endingPosition = source.Length;

        tokens.Add(new HeaderToken(endingPosition, true, tagEndingLength));

        return true;
    }
}