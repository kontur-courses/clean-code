namespace Markdown.Tokens;

public class Token : IToken
{
    public Token(int startIndex, int endIndex, string value)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
        Value = value;
    }

    public int StartIndex { get; }
    public int EndIndex { get; }
    public string Value { get; }
}