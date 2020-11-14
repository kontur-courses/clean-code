using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization;

namespace MarkdownParser.Concrete.Italic
{
    public sealed class ItalicElementFactory : MarkdownElementFactory<MarkdownElementItalic>,
        IMarkdownCollectorDependent
    {
        private MarkdownCollector markdownCollector;

        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementItalic parsed)
        {
            var openingToken = (ItalicToken) context.CurrentToken;
            if (markdownCollector.TryCollectUntil<ItalicToken>(context,
                ItalicToken.CanBeClosing, out var closingToken, out var inner) &&
                (!openingToken.Position.InsideWord() && !closingToken.Position.OnWordBorder() ||
                 inner.All(t => !t.RawValue.Contains(" "))))
            {
                parsed = new MarkdownElementItalic(openingToken, inner.ToArray(), closingToken);
                return true;
            }

            parsed = default;
            return false;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is ItalicToken italic && ItalicToken.CanBeOpening(italic);

        public void SetCollector(MarkdownCollector collector) =>
            markdownCollector = collector;
    }
}