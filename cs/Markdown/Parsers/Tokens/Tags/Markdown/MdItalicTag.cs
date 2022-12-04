using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdItalicTag : MdPairedTag
    {
        public MdItalicTag(MdPairedTag startTag) : base(startTag, "_")
        {
        }
        public MdItalicTag(TagPosition tagPosition) : base(tagPosition, "_")
        {
        }

        public override IToken ToHtml() => new HtmlItalicTag(Position);
    }
}
