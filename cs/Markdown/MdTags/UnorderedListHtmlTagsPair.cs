using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class UnorderedListHtmlTagsPair : IHtmlTagsPair
{
    public string HtmlOpenTag => "<ul>";

    public string HtmlCloseTag => "</ul>";
}
