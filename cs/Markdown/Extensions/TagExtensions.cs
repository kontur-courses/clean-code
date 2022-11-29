using Markdown.Primitives;

namespace Markdown.Extensions;

public static class TagExtensions
{
    public static TagNode ToTagNode(this Tag tag) => new TagNode(tag);
}