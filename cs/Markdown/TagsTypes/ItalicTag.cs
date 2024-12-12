namespace Markdown.TagsType;

public class ItalicTag : ITagsType
{
    public string MarkdownTag { get; } = "_";
    public bool HasPair { get; set; }

    public string GetHtmlOpenTag => "<em>";

    public string GetHtmlCloseTag => "</em>";
}