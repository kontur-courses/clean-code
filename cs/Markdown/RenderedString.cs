namespace Markdown;

public class RenderedString
{
    public int start { private set; get; }
    public int end { private set; get; }
    public string text { private set; get; }

    public RenderedString(string text, int start, int end)
    {
        this.text = text;
        this.start = start;
        this.end = end;
    }
}