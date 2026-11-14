using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.MDParsing
{
    public class MarkdownParser
    {
        private const string HeaderPrefix = "# ";
        private const string ListItemPrefix = "- ";

        private readonly record struct Line(string Text, string Ending);

        public List<MarkdownNode> Parse(string input)
        {
            var result = new List<MarkdownNode>();
            ListNode? currentList = null;
            string listEnding = string.Empty; 

            foreach (var line in SplitLines(input))
            {
                var tokens = MarkdownTokenizer.Tokenize(line.Text);

                if (TryStripPrefix(tokens, ListItemPrefix, out var itemTokens))
                {
                    if (currentList == null)
                    {
                        currentList = new ListNode();
                        result.Add(currentList);
                    }
                    currentList.Items.Add(new ListItemNode(ParseInlineFromTokens(itemTokens)));
                    listEnding = line.Ending;
                    continue;
                }

                EndList(result, ref currentList, listEnding);

                if (TryStripPrefix(tokens, HeaderPrefix, out var headerTokens))
                    result.Add(new HeaderNode { Children = ParseInlineFromTokens(headerTokens) });
                else
                    result.AddRange(ParseInlineFromTokens(tokens));

                AddLineEnding(result, line.Ending);
            }

            EndList(result, ref currentList, listEnding);
            return result;
        }

        private static IEnumerable<Line> SplitLines(string input)
        {
            int start = 0;
            while (true)
            {
                int newline = input.IndexOf('\n', start);
                if (newline < 0)
                {
                    yield return new Line(input.Substring(start), string.Empty);
                    yield break;
                }

                bool hasCarriageReturn = newline > start && input[newline - 1] == '\r';
                int textEnd = hasCarriageReturn ? newline - 1 : newline;
                yield return new Line(
                    input.Substring(start, textEnd - start),
                    hasCarriageReturn ? "\r\n" : "\n");

                start = newline + 1;
            }
        }

        private static bool TryStripPrefix(List<Token> tokens, string prefix, out List<Token> content)
        {
            content = tokens;

            if (tokens.Count == 0 || tokens[0].IsEscaped
                || !tokens[0].Text.StartsWith(prefix, StringComparison.Ordinal))
                return false;

            bool hasContent = tokens.Count > 1 || tokens[0].Text.Length > prefix.Length;
            if (!hasContent) return false;

            content = new List<Token>(tokens);
            content[0] = Token.CreateText(tokens[0].Text.Substring(prefix.Length));
            return true;
        }

        private static void EndList(List<MarkdownNode> result, ref ListNode? list, string ending)
        {
            if (list == null) return;
            AddLineEnding(result, ending);
            list = null;
        }

        private static void AddLineEnding(List<MarkdownNode> result, string ending)
        {
            if (ending.Length > 0) result.Add(new TextNode(ending));
        }

        private static List<InlineNode> ParseInlineFromTokens(List<Token> tokens) =>
            new InlineParser(tokens).ParseAll();
    }
}