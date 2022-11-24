using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Html;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdItalicTag : MdPairedTag
    {
        public MdItalicTag(TagPosition position) : base(position, "_")
        {
        }

        public override IToken ToHtml() => new HtmlItalicTag(position);

        public override bool IsValidTag(string data, int position)
        {
            return base.IsValidTag(data, position) || 
                   data.Length > position && !char.IsDigit(data[position]);
        }
    }
}
