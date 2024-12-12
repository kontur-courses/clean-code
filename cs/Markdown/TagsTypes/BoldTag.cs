namespace Markdown.TagsType;

public class BoldTag : ITagsType
{
    public string MarkdownTag { get; } = "__";
    public bool HasPair { get; set; }

    public string GetHtmlOpenTag => "<strong>";

    public string GetHtmlCloseTag => "</strong>";
}