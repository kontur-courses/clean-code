using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdBoldTag : MdPairedTag
    {
        public MdBoldTag(TagPosition position) : base(position, "__")
        {
        }

        public override IToken ToHtml() => new HtmlBoldTag(position);

        public override bool IsValidTag(string data, int position)
        {
            return base.IsValidTag(data, position);
        }
    }
}
