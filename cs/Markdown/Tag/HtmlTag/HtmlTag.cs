namespace Markdown;

public class HtmlTag(bool isPairedTag, string startTag,  string? endTag = null)
{
    public string StartTag { get; } = startTag;
    public string? EndTag { get; } = endTag;
    public bool IsPairedTag { get; } = isPairedTag;
}