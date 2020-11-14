using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization;

namespace MarkdownParser.Concrete.Header
{
    public class HeaderElementFactory : MarkdownElementFactory<MarkdownElementHeader>, IMarkdownCollectorDependent
    {
        private MarkdownCollector collector;
        
        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementHeader parsed)
        {
            var inner = collector.CreateElementsFrom(context.NextTokens).ToArray();
            parsed = new MarkdownElementHeader(context.CurrentToken, inner);
            return true;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is HeaderToken header && header.Position.OnParagraphStart();

        public void SetCollector(MarkdownCollector collector) => this.collector = collector;
    }
}