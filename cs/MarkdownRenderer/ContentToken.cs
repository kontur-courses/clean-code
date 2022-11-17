namespace MarkdownRenderer;

public class ContentToken : Token
{
    public int ContentStart { get; }
    public int ContentEnd { get; }
    public int ContentLength => ContentEnd - ContentStart + 1;

    public ContentToken(int start, int end, int contentStart, int contentEnd) : base(start, end)
    {
        ContentStart = contentStart;
        ContentEnd = contentEnd;
    }
}