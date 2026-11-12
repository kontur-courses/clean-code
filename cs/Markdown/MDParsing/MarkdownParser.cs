using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.MDParsing
{
    public class MarkdownParser
    {
        private const string HeaderPrefix = "# ";
        private const string ListItemPrefix = "- ";

        public List<MarkdownNode> Parse(string input)
        {
            if (string.IsNullOrEmpty(input)) return new List<MarkdownNode>();

            var result = new List<MarkdownNode>();
            ListNode? currentList = null;

            foreach (var line in SplitLines(input))
            {
                var tokens = MarkdownTokenizer.Tokenize(line);

                if (TryStripPrefix(tokens, ListItemPrefix, out var itemTokens))
                {
                    if (currentList == null)
                    {
                        currentList = new ListNode();
                        result.Add(currentList);
                    }
                    currentList.Items.Add(new ListItemNode(ParseInlineFromTokens(itemTokens)));
                    continue;
                }

                currentList = null; 

                if (TryStripPrefix(tokens, HeaderPrefix, out var headerTokens))
                    result.Add(new HeaderNode { Children = ParseInlineFromTokens(headerTokens) });
                else
                    result.AddRange(ParseInlineFromTokens(tokens));
            }

            return result;
        }

        private static IEnumerable<string> SplitLines(string input) =>
            input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                 .Select(line => line.Trim())
                 .Where(line => line.Length > 0);


        private static bool TryStripPrefix(List<Token> tokens, string prefix, out List<Token> content)
        {
            content = tokens;

            if (tokens.Count == 0 || tokens[0].IsEscaped || !tokens[0].Text.StartsWith(prefix))
                return false;

            content = new List<Token>(tokens);
            content[0] = Token.CreateText(tokens[0].Text.Substring(prefix.Length));
            return true;
        }

        private static List<InlineNode> ParseInlineFromTokens(List<Token> tokens) =>
            new InlineParser(tokens).ParseAll();
    }
}