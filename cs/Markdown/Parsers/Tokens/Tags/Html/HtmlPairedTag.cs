using Markdown.Parsers.Tokens.Tags.Enum;

namespace Markdown.Parsers.Tokens.Tags.Html
{
    public abstract class HtmlPairedTag : PairedTag
    {
        protected HtmlPairedTag(TagPosition position, string text) : base(position, text)
        {
        }

        public override string ToString() =>
            position == TagPosition.Start ? text : text.Insert(1, "/");
    }
}
