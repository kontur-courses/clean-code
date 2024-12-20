
using Markdown.MdTags;
using Markdown.MdTags.Interfaces;

namespace Markdown.MdTagHandlers;

internal class PairTagsIntersectionHandler : IPairTagsIntersectionHandler
{
    public Dictionary<IMdTag, List<TagReplacementsPair>> RemoveIntersections(
        Dictionary<IMdTag, List<TagReplacementsPair>> tagsPairs)
    {
        var allTagsPairs = Flatten(tagsPairs);
        var result = tagsPairs.ToDictionary(kv => kv.Key, kv => new List<TagReplacementsPair>());
        var previousTag = default(IMdTag);
        var previousTagReplacementPair = default(TagReplacementsPair);

        foreach (var (tag, tagReplacementPair) in Flatten(tagsPairs))
        {
            if (previousTag == null)
            {
                result[tag].Add(tagReplacementPair);
            }
            else if (HavePartialIntersection(previousTagReplacementPair, tagReplacementPair))
            {
                result[previousTag].RemoveAt(result[previousTag].Count - 1);
            }
            else if (!IsBoldInsideItalic(previousTag, previousTagReplacementPair, tag, tagReplacementPair))
            {
                result[tag].Add(tagReplacementPair);
            }

            previousTag = tag;
            previousTagReplacementPair = tagReplacementPair;
        }

        return result;
    }

    private (IMdTag Tag, TagReplacementsPair TagReplacementsPair)[] Flatten(
        Dictionary<IMdTag, List<TagReplacementsPair>> tagsPairs)
    {
        var indices = tagsPairs.ToDictionary(kv => kv.Key, kv => 0);
        var totalCount = tagsPairs.Sum(kv => kv.Value.Count);
        var result = new (IMdTag, TagReplacementsPair)[totalCount];

        for (var i = 0; i < totalCount; i++)
        {
            var leftmostTag = tagsPairs.Keys
                .Where(tag => indices[tag] < tagsPairs[tag].Count)
                .MinBy(tag => tagsPairs[tag][indices[tag]].OpenTag.Index)!;

            result[i] = (leftmostTag, tagsPairs[leftmostTag][indices[leftmostTag]]);
            indices[leftmostTag]++;
        }

        return result;
    }

    private bool IsBoldInsideItalic(
        IMdTag previousTag,
        TagReplacementsPair previousTagReplacementPair,
        IMdTag tag,
        TagReplacementsPair tagReplacementPair)
    {
        return HaveFullIntersection(previousTagReplacementPair, tagReplacementPair)
            && previousTag is ItalicTextMdTag
            && tag is BoldTextMdTag;
    }

    private bool HavePartialIntersection(TagReplacementsPair left, TagReplacementsPair right)
    {
        return left.CloseTag.EndIndex >= right.OpenTag.Index
            && left.CloseTag.EndIndex < right.CloseTag.EndIndex;
    }

    private bool HaveFullIntersection(TagReplacementsPair left, TagReplacementsPair right)
    {
        return left.CloseTag.EndIndex >= right.OpenTag.Index
            && left.CloseTag.EndIndex >= right.CloseTag.EndIndex;
    }
}
