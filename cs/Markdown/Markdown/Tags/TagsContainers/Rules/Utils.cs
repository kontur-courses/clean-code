using Markdown.Tokens;

namespace Markdown.Tags.TagsContainers.Rules;

public static class Utils
{
    public static bool IsTagsInsideOneWord(TagToken firstTag, TagToken secondTag,
        Dictionary<int, TagToken> parsedTokens)
    {
        var nextTokenAfterFirstTag = parsedTokens[firstTag.EndIndex + 1];

        return nextTokenAfterFirstTag.EndIndex == secondTag.StartIndex - 1
               && !nextTokenAfterFirstTag.ToString().Contains(' ');
    }
}