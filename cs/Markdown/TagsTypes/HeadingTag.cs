namespace Markdown.TagsType;

public class HeadingTag : ITagsType
{
    public string MarkdownTag { get; } = "#";
    public bool HasPair { get; set; } = true;

    public string GetHtmlOpenTag => "<h1>";

    public string GetHtmlCloseTag => "</h1>";
}