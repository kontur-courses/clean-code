using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Link
{
    public class LinkElementFactory : MdPairElementFactory<MdElementLink, LinkToken, LinkToken>
    {
        protected override MdElementLink Create(LinkToken opening, Token[] innerTokens, LinkToken closing)
        {
            var content = innerTokens.Length == 1
                ? innerTokens[0]
                : new TextToken(
                    innerTokens[0].StartPosition,
                    string.Join(string.Empty, innerTokens.Select(x => x.RawValue)));
            return new MdElementLink(content);
        }

        protected override bool CanCreate(LinkToken opening, Token[] inner, LinkToken closing)
        {
            return base.CanCreate(opening, inner, closing)
                   && opening.CanBeOpening()
                   && closing.CanBeClosing();
        }
    }
}