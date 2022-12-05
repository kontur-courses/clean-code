using System;

namespace Markdown.Parsers
{
    public class MarkdownParser
    {
        private readonly ParsedDocument document = new ParsedDocument();

        public MarkdownParser(string markdownText)
        {
            var lines = markdownText.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                var parsingLine = new MarkdownParsingLine(line);
                var lineTokens = parsingLine.Parse();
                document.TextBlocks.Add(new ParsedTextBlock(lineTokens));
            }
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
