namespace Markdown.Tag;

public class HtmlTag : ITag
{
    public string OpeningSeparator { get; }
    public string CloseSeparator { get; }

    public HtmlTag(string openingSeparator, string endingSeparator)
    {
        OpeningSeparator = openingSeparator;
        CloseSeparator = endingSeparator;
    }
}