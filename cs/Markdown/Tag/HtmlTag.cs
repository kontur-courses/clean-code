namespace Markdown.Tag;

public class HtmlTag : ITag
{
    public string OpeningSeparator { get; }
    public string CloseSeparator { get; }
    public bool IsPaired { get; }

    public HtmlTag(string openingSeparator, string endingSeparator, bool isPaired)
    {
        OpeningSeparator = openingSeparator;
        CloseSeparator = endingSeparator;
        IsPaired = isPaired;
    }
}