using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Markdown;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public class HtmlBoldTag : HtmlPairedTag
    {
        public HtmlBoldTag(TagPosition position) : base(position, "<strong>")
        {
        }

        public override IToken ToMarkdown() => new MdBoldTag(Position);
    }
}
