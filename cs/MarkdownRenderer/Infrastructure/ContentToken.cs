namespace MarkdownRenderer.Infrastructure;

public class ContentToken : Token
{
    public int ContentStart { get; }
    public int ContentEnd { get; }
    public int ContentLength => ContentEnd - ContentStart + 1;

    public ContentToken(int start, int end, int offsetLeft, int offsetRight) : base(start, end)
    {
        ContentStart = Start + offsetLeft;
        ContentEnd = End - offsetRight;
    }
}