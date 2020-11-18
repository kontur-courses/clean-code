using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Header
{
    public class HeaderElementFactory : MdElementFactory<MarkdownElementHeader, HeaderToken>,
        IMarkdownCollectorDependent
    {
        private MarkdownCollector collector;

        public void SetCollector(MarkdownCollector newCollector) => collector = newCollector;

        public override MarkdownElementHeader Create(HeaderToken token, Token[] nextTokens)
        {
            var inner = collector.CreateElementsFrom(nextTokens).ToArray();
            return new MarkdownElementHeader(token, inner);
        }
    }
}