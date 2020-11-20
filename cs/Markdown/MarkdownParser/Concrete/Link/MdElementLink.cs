using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Link
{
    public class MdElementLink : MarkdownElement
    {
        public readonly Token Link;
        public readonly Token Text;

        public MdElementLink(Token textOpening, Token text, Token textClosing,
            Token linkOpening, Token link, Token linkClosing)
            : base(new[] {textOpening, text, textClosing, linkOpening, link, linkClosing})
        {
            Link = link;
            Text = text;
        }

        public MdElementLink(Token opening, Token link, Token closing) : base(new[] {opening, link, closing})
        {
            Link = link;
            Text = link;
        }
    }
}