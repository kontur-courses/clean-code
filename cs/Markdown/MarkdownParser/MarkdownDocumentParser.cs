using System.Linq;
using MarkdownParser.Infrastructure;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization;

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
            var tokens = tokenizer.Tokenize(rawMarkdown).ToArray();
            return new MarkdownDocument(collector.CreateElementsFrom(tokens));
        }
    }
}