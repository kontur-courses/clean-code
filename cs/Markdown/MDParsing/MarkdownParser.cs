using Markdown.Abstractions;
using Markdown.MDElements;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        public List<MarkdownNode> Parse(string input)
        {
            if (string.IsNullOrEmpty(input)) return new List<MarkdownNode>();

            var result = new List<MarkdownNode>();
            var lines = input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;

                if (trimmedLine.StartsWith("# "))
                {
                    var header = new HeaderNode();
                    header.Children = ParseInline(trimmedLine.Substring(2));
                    result.Add(header);
                }
                else
                {
                    result.AddRange(ParseInline(trimmedLine));
                }
            }
            return result;
        }

        private List<InlineNode> ParseInline(string text)
        {
            var tokens = MarkdownTokenizer.Tokenize(text);

            var parser = new InlineParser(tokens);
            return parser.ParseAll();
        }
    }
}