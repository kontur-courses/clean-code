using Markdown.Parsers.Tokens;

namespace Markdown.Parsers
{
    public class MarkdownParser
    {
        private readonly ParsedDocument document = new ParsedDocument();

        //TODO: new line as \r\n
        private readonly NewLineToken newLineToken = new NewLineToken();
        public MarkdownParser(string markdownText)
        {
            var lines = markdownText.Split(newLineToken.ToString());
            foreach (var line in lines)
            {
                var parsingLine = new MdParsingLine(line);
                var lineTokens = parsingLine.Parse();
                document.TextBlocks.Add(new ParsedTextBlock(lineTokens));
            }
        }

        public ParsedDocument GetParsedDocument() => document;
    }
}
