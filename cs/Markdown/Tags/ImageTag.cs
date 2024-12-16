namespace Markdown.Tags;

/// <summary>
/// Тег для картинки
/// </summary>
public class ImageTag : ITag
{
    public string MdTag { get; } = "![";

    public string MdClosingTag { get; } = ")";

    public string HtmlTag { get; } = "img";

    public IReadOnlyCollection<ITag> DisallowedChildren => new List<ITag>()
        { new CursiveTag(), new HeaderTag(), new ImageTag(), new StrongTag() };

    public bool SelfClosing { get; } = false;

    public static Dictionary<string, string> GetHtmlTadAttributes(string tagContents)
    {
        var attributes = new Dictionary<string, string>();

        var altStart = tagContents.IndexOf("![", StringComparison.Ordinal) + 2;
        var altEnd = tagContents.IndexOf(']', altStart);
        var srcStart = tagContents.IndexOf('(', altEnd) + 1;
        var srcEnd = tagContents.IndexOf(')', srcStart);

        if (altStart < 2 || altEnd <= altStart || srcStart <= altEnd || srcEnd <= srcStart) return attributes;
        attributes["alt"] = tagContents.Substring(altStart, altEnd - altStart);
        attributes["src"] = tagContents.Substring(srcStart, srcEnd - srcStart);

        return attributes;
    }

    public bool Matches(ITag tag)
    {
        return tag is ImageTag;
    }
}