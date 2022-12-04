using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlParagraphTag : HtmlPairedTag
    {
        public HtmlParagraphTag(TagPosition position) : base(position, "<p>")
        {
        }
    }
}
