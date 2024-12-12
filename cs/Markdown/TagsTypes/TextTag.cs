namespace Markdown.TagsType;

public class TextTag : ITagsType
{
    public string MarkdownTag { get; }
    public bool HasPair { get; set; }
    public string GetHtmlOpenTag { get; }
    public string GetHtmlCloseTag { get; }
}