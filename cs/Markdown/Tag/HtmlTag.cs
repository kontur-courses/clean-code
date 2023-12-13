namespace Markdown.Tag;

public class HtmlTag : ITag
{
    public string Start { get; }
    public string End { get; }

    public HtmlTag(string start, string end)
    {
        Start = start;
        End = end;
    }
}