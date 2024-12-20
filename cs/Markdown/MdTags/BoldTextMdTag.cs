using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class BoldTextMdTag : IMdTag, IHtmlTagsPair
{
    private const string MdTag = "__";

    public string HtmlOpenTag => "<strong>";

    public string HtmlCloseTag => "</strong>";

    public TagType TagType => TagType.Pair;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        tagLength = MdTag.Length;
        return TagSearchHelper.IsSubstring(mdString, MdTag, startIndex);
    }
}
