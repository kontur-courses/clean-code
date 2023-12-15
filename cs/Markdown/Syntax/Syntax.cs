using Markdown.Token;
using Markdown.Tag;

namespace Markdown.Syntax;

public class Syntax : ISyntax
{
    private readonly Dictionary<TagType, ITag> tagTypeToHtml = new Dictionary<TagType, ITag>
    {
        { TagType.Italic, new HtmlTag("<em>", "</em>") }, { TagType.Bold, new HtmlTag("<strong>", "</strong>") },
        { TagType.Header, new HtmlTag("<h1>", "</h1>") }
    };

    private readonly Dictionary<string, TagType> markdownToTagType = new Dictionary<string, TagType>
    {
        { "#", TagType.Header }, { "_", TagType.Italic }, { "__", TagType.Bold }
    };

    public ITag ConvertTag(TagType type)
    {
        return tagTypeToHtml[type];
    }

    public TagType GetTagType(string tag)
    {
        return markdownToTagType[tag];
    }
}