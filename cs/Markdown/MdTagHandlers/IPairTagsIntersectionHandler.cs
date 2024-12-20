using Markdown.MdTags.Interfaces;

namespace Markdown.MdTagHandlers;

internal interface IPairTagsIntersectionHandler
{
    public Dictionary<IMdTag, List<TagReplacementsPair>> RemoveIntersections(
        Dictionary<IMdTag, List<TagReplacementsPair>> tagsIndices);
}
