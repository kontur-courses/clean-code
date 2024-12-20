using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class OrderedListItemMdTag : IGroupedMdTag, IHtmlTagsPair
{
    private const string MdTagEnd = ".";

    private readonly OrderedListHtmlTagsPair orderedListTags = new();

    public string HtmlOpenTag => "<li>";

    public string HtmlCloseTag => "</li>";

    public TagType TagType => TagType.FullLine;

    public IHtmlTagsPair GroupHtmlTagsPair => orderedListTags;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        var isNumber = TagSearchHelper.IsNumber(mdString, startIndex, out var numberLength);
        tagLength = numberLength + MdTagEnd.Length + 1;
        return isNumber
            && TagSearchHelper.IsSubstring(mdString, MdTagEnd, startIndex + numberLength)
            && TagSearchHelper.IsWhitespaceOrStringEnd(mdString, startIndex + numberLength + MdTagEnd.Length);
    }
}
