using Markdown.Tag;

namespace Markdown.Syntax;

public class Syntax : ISyntax
{
    private readonly Dictionary<TagType, ITag> markdownToHtml = new Dictionary<TagType, ITag>
    {
        { TagType.Italic, new HtmlTag("<em>", "</em>") }, { TagType.Bold, new HtmlTag("<strong>", "</strong>") },
        { TagType.Header, new HtmlTag("<h1>", "</h1>") }
    };

    public ITag ConvertTag(TagType type)
    {
        return markdownToHtml[type];
    }
}