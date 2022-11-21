namespace Markdown.Tags;

public class MarkdownTag
{
    public MarkdownTag(string withStart, string withEnd)
    {
        WithStart = withStart;
        WithEnd = withEnd;
    }

    public MarkdownTag(string name) : this(name, name)
    {
    }

	public string WithStart { get; }

    public string WithEnd { get; }
}