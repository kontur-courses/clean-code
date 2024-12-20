using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class ItalicTextMdTag : IMdTag, IHtmlTagsPair
{
    private const string MdTag = "_";

    public string HtmlOpenTag => "<em>";

    public string HtmlCloseTag => "</em>";

    public TagType TagType => TagType.Pair;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        tagLength = MdTag.Length;
        return TagSearchHelper.IsSubstring(mdString, MdTag, startIndex);
    }
}
