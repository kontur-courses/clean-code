using Markdown.Tags;

namespace Markdown.Extensions;

public static class TagExtension
{
    public static bool IsOpeningTag(this Tag tag)
    {
        return tag.TagContent == tag.ReplacementForOpeningTag;
    }

    public static bool IsClosingTag(this Tag tag)
    {
        return tag.TagContent == tag.ReplacementForClosingTag;
    }
}