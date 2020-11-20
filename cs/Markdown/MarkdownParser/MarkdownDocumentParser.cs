using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization.Workers;

namespace MarkdownParser
{
    public class MarkdownDocumentParser
    {
        private readonly MarkdownCollector collector;
        private readonly Tokenizer tokenizer;

        public MarkdownDocumentParser(Tokenizer tokenizer, MarkdownCollector markdownCollector)
        {
            this.tokenizer = tokenizer;
            collector = markdownCollector;
        }

        public MarkdownDocument Parse(string rawMarkdown)
        {
            var documentLinesEnumerable = tokenizer.Tokenize(rawMarkdown)
                .Select(p => collector.CreateElementsFrom(p.Tokens))
                .Select(t => new MarkdownDocumentLine(t));
            return new MarkdownDocument(documentLinesEnumerable);
        }
    }
}