using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdBoldTag : MdPairedTag
    {
        public MdBoldTag(MdParsingLine context = null) : base(context, "__")
        {
        }

        public MdBoldTag(TagPosition tagPosition) : base(tagPosition, "__")
        {
        }

        public override IToken ToHtml() => new HtmlBoldTag(Position);
    }
}
