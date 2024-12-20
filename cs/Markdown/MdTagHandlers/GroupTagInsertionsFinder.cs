using Markdown.MdTags.Interfaces;

namespace Markdown.MdTagHandlers;

internal class GroupTagInsertionsFinder : IGroupTagInsertionsFinder
{
    public IEnumerable<SubstringReplacement> GetGroupTagReplacements(
        string mdString,
        IHtmlTagsPair groupTag,
        IEnumerable<TagReplacementsPair> tagPairs)
    {
        using var enumerator = tagPairs.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var previous = enumerator.Current;
        yield return new SubstringReplacement(previous.OpenTag.Index, 0, groupTag.HtmlOpenTag);

        while (enumerator.MoveNext())
        {
            var previousEndIndex = previous.CloseTag.EndIndex;
            var currentStartIndex = enumerator.Current.OpenTag.Index;
            var lengthBetweenTags = currentStartIndex - previousEndIndex;

            if (!TagSearchHelper.IsWhiteSpaceSubstring(mdString, previousEndIndex, lengthBetweenTags))
            {
                yield return new SubstringReplacement(previousEndIndex, 0, groupTag.HtmlCloseTag);
                yield return new SubstringReplacement(currentStartIndex - 1, 0, groupTag.HtmlOpenTag);
            }

            previous = enumerator.Current;
        }

        yield return new SubstringReplacement(previous.CloseTag.EndIndex, 0, groupTag.HtmlCloseTag);
    }
}
