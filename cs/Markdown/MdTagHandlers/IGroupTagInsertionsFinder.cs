using Markdown.MdTags.Interfaces;

namespace Markdown.MdTagHandlers;

internal interface IGroupTagInsertionsFinder
{
    public IEnumerable<SubstringReplacement> GetGroupTagReplacements(
        string mdString, IHtmlTagsPair groupTag, IEnumerable<TagReplacementsPair> tagPairs);
}
