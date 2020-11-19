using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser.Concrete.Bold
{
    public sealed class BoldElementFactory : MdPairElementFactory<MarkdownElementBold, BoldToken, BoldToken>,
        IMarkdownCollectorDependent
    {
        private MarkdownCollector collector;

        public void SetCollector(MarkdownCollector newCollector) => this.collector = newCollector;

        protected override MarkdownElementBold Create(BoldToken opening, Token[] innerTokens, BoldToken closing)
        {
            var innerElements = collector.CreateElementsFrom(innerTokens).ToArray();
            return new MarkdownElementBold(opening, innerElements, innerTokens, closing);
        }

        protected override bool CanCreate(BoldToken opening, Token[] inner, BoldToken closing) =>
            base.CanCreate(opening, inner, closing) &&
            !opening.Position.InsideWord() &&
            !closing.Position.OnWordBorder() ||
            inner.All(t => !t.RawValue.Contains(" "));
    }
}