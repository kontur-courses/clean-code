using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Italic
{
    public sealed class ItalicElementFactory : MdPairElementFactory<MarkdownElementItalic, ItalicToken, ItalicToken>
    {
        protected override MarkdownElementItalic Create(ItalicToken opening, Token[] innerTokens, ItalicToken closing) =>
            new MarkdownElementItalic(opening, innerTokens, closing);

        protected override bool CanCreate(ItalicToken opening, Token[] inner, ItalicToken closing) =>
            base.CanCreate(opening, inner, closing) &&
            !opening.Position.InsideWord() &&
            !closing.Position.OnWordBorder() ||
            inner.All(t => !t.RawValue.Contains(" "));
    }
}