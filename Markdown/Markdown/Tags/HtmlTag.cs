namespace Markdown.Tags;

public class HtmlTag
{
    public HtmlTag(string open, string close)
    {
        Open = open;
        Close = close;
    }

    public HtmlTag(string name) : this($"<{name}>", $"</{name}>")
    {
    }

    public string Open { get; }

    public string Close { get; }
}