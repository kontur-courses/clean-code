using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.MDParsing
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

                var tokens = MarkdownTokenizer.Tokenize(trimmedLine);

                if (IsHeader(tokens))
                {
                    var headerContent = GetHeaderContent(tokens);
                    var headerNode = new HeaderNode();
                    headerNode.Children = ParseInlineFromTokens(headerContent);
                    result.Add(headerNode);
                }
                else
                {
                    result.AddRange(ParseInlineFromTokens(tokens));
                }
            }
            return result;
        }


        private bool IsHeader(List<Token> tokens) =>
            tokens.Count > 0
            && tokens[0].Type == TokenType.Text
            && tokens[0].Text.StartsWith("# ");

        private List<Token> GetHeaderContent(List<Token> tokens)
        {
            var result = new List<Token>(tokens);
            result[0] = Token.CreateText(result[0].Text.Substring(2));
            return result;
        }


        private List<InlineNode> ParseInlineFromTokens(List<Token> tokens)
        {
            var parser = new InlineParser(tokens);
            return parser.ParseAll();
        }

    }
}