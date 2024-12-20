using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class OrderedListHtmlTagsPair : IHtmlTagsPair
{
    public string HtmlOpenTag => "<ol>";

    public string HtmlCloseTag => "</ol>";
}
