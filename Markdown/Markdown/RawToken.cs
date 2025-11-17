namespace Markdown;

public class RawToken
{
    public TokenType Type { get; set; }
    public int TokenMarkLength { get; set; }
    public int StartIndex { get; set; }

    public RawToken(TokenType type, int tokenMarkLength, int startIndex)
    {
        this.Type = type;
        this.TokenMarkLength = tokenMarkLength;
        this.StartIndex = startIndex;
    }
}