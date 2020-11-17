using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

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
            var paragraphData = tokenizer.Tokenize(rawMarkdown).ToArray();
            var document = MarkdownDocument.Empty;
            foreach (var paragraph in paragraphData)
            {
                var tokens = PairedTokenWorker.FixCrossingTokens(paragraph.Tokens).ToArray();
                var elements = collector.CreateElementsFrom(tokens);
                var line = new MarkdownDocumentLine(elements);
                document.Add(line);
            }

            return document;
        }
    }
}