namespace Markdown.Tags;

public class HtmlTag
{
    public HtmlTag(string withStart, string withEnd)
    {
        WithStart = withStart;
        WithEnd = withEnd;
    }

    public HtmlTag(string name) : this($"<{name}>", $"</{name}>")
    {
    }

    public string WithStart { get; }

    public string WithEnd { get; }
}