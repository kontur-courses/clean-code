using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class HeaderMdTag : IMdTag, IHtmlTagsPair
{
    private const string MdTag = "#";

    public string HtmlOpenTag => "<h1>";

    public string HtmlCloseTag => "</h1>";

    public TagType TagType => TagType.FullLine;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        tagLength = MdTag.Length + 1;
        return TagSearchHelper.IsNewLineStart(mdString, startIndex)
            && TagSearchHelper.IsSubstring(mdString, MdTag, startIndex)
            && TagSearchHelper.IsWhitespaceOrStringEnd(mdString, startIndex + MdTag.Length);
    }
}
