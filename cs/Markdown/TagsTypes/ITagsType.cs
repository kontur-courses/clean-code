namespace Markdown.TagsType;

public interface ITagsType
{
    public string MarkdownTag { get; }
    public bool HasPair { get; set; }
    public string GetHtmlOpenTag { get; }
    public string GetHtmlCloseTag { get; }
}