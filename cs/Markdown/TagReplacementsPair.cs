using Markdown.MdTags.Interfaces;

namespace Markdown;

internal record struct TagReplacementsPair(SubstringReplacement OpenTag, SubstringReplacement CloseTag)
{
    public TagReplacementsPair GetHtmlReplacements(IHtmlTagsPair tag)
    {
        return new TagReplacementsPair(
            OpenTag with { Replacement = tag.HtmlOpenTag },
            CloseTag with { Replacement = tag.HtmlCloseTag });
    }

    public readonly IEnumerable<SubstringReplacement> Flatten()
    {
        yield return OpenTag;
        yield return CloseTag;
    }
}
