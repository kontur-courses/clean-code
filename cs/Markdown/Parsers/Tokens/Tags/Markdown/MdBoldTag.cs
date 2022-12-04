using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdBoldTag : MdPairedTag
    {
        public MdBoldTag(MdPairedTag startTag) : base(startTag, "__")
        {
        }

        public MdBoldTag(TagPosition tagPosition) : base(tagPosition, "__")
        {
        }

        public override IToken ToHtml() => new HtmlBoldTag(Position);
    }
}
