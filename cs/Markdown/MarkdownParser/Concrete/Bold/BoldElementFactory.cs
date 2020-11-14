using System.Linq;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization;

namespace MarkdownParser.Concrete.Bold
{
    public sealed class BoldElementFactory : MarkdownElementFactory<MarkdownElementBold>, IMarkdownCollectorDependent
    {
        private MarkdownCollector collector;

        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementBold parsed)
        {
            var opening = (BoldToken) context.CurrentToken;
            // TODO holy shit...
            if (collector.TryCollectUntil<BoldToken>(context, BoldToken.CanBeClosing, out var closing, out var inner) &&
                inner.Count != 0 &&
                (!opening.Position.InsideWord() && !closing.Position.OnWordBorder() ||
                 inner.All(t => !t.RawValue.Contains(" "))))
            {
                var innerElements = collector.CreateElementsFrom(inner.ToArray());
                var allElemTokens = inner.Prepend(context.CurrentToken).Append(closing).ToArray();
                parsed = new MarkdownElementBold(innerElements.ToArray(), allElemTokens);
                return true;
            }

            parsed = default;
            return false;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is BoldToken bold && BoldToken.CanBeOpening(bold);

        public void SetCollector(MarkdownCollector collector) =>
            this.collector = collector;
    }
}