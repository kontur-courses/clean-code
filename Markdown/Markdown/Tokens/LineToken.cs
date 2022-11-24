namespace Markdown.Tokens;

public class LineToken : Token
{
    public LineToken(string line) : base(string.Empty, string.Empty, TokenType.Line)
    {
        FirstPosition = 0;
        Length = line.Length;
    }
}