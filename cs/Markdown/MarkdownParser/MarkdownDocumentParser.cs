using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization.Models;
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
            var paragraphData = tokenizer.Tokenize(rawMarkdown).ToArray();
            var document = MarkdownDocument.Empty;
            foreach (var paragraph in paragraphData)
            {
                var tokens = paragraph.Tokens.FixCrossingTokens().ToArray();
                var pairsResolvedTokens = TokenPairsResolver.ResolvePairs(tokens)
                    .Select(t => t is TokenPair p && p.Inner.Length == 0
                        ? TokenCreator.CreateDefault(p.Opening.StartPosition, p.Opening.RawValue + p.Closing.RawValue)
                        : t)
                    .ToArray();
                var elements = collector.CreateElementsFrom(pairsResolvedTokens);
                var line = new MarkdownDocumentLine(elements);
                document.Add(line);
            }

            return document;
        }
    }
}