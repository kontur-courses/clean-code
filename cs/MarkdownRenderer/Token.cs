namespace MarkdownRenderer;

public class Token
{
    public int Start { get; }
    public int End { get; }
    public int Length => End - Start + 1;

    public Token(int start, int end)
    {
        Start = start;
        End = end;
    }
}