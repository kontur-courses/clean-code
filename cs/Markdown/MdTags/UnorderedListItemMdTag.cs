using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class UnorderedListItemMdTag : IGroupedMdTag, IHtmlTagsPair
{
    private const string MdTag = "-";

    private readonly UnorderedListHtmlTagsPair unorderedListTags = new();

    public string HtmlOpenTag => "<li>";

    public string HtmlCloseTag => "</li>";

    public TagType TagType => TagType.FullLine;

    public IHtmlTagsPair GroupHtmlTagsPair => unorderedListTags;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        tagLength = MdTag.Length + 1;
        return TagSearchHelper.IsNewLineStart(mdString, startIndex)
            && TagSearchHelper.IsSubstring(mdString, MdTag, startIndex)
            && TagSearchHelper.IsWhitespaceOrStringEnd(mdString, startIndex + MdTag.Length);
    }
}
