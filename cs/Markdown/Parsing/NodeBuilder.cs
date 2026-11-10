using System.Text;
using Markdown.Domain;

namespace Markdown.Parsing;

internal static class NodeBuilder
{
    private const string HeaderPrefix = "# ";

    public static IReadOnlyList<Node> Build(string markdown)
    {
        var nodes = new List<Node>();
        var paragraph = new StringBuilder();

        using var reader = new StringReader(markdown);

        while (reader.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                FlushParagraph();
                continue;
            }

            if(line.StartsWith(HeaderPrefix))
            {
                FlushParagraph();
                nodes.Add(BuildLine(line, true));
                continue;
            }

            if (paragraph.Length > 0)
            {
                paragraph.Append(' ');
            }

            paragraph.Append(line.Trim());
        }

        FlushParagraph();
        return nodes;

        void FlushParagraph()
        {
            if (paragraph.Length > 0)
            {
                nodes.Add(BuildLine(paragraph.ToString()));
                paragraph.Clear();
            }
        }
    }

    private static Node BuildLine(string line, bool isHeader = false)
    {
        var startIndex = isHeader ? HeaderPrefix.Length : 0;

        var tokens = InlineTokenizer.Tokenize(line, startIndex);
        var pairs = DelimiterMatcher.FindPairs(tokens, line);

        var children = BuildChildren(line, startIndex, tokens, pairs);

        return isHeader
            ? new HeaderNode { Children = children }
            : new ParagraphNode { Children = children };
    }

    private static List<Node> BuildChildren(
        string text,
        int startIndex,
        IReadOnlyList<Token> tokens,
        IReadOnlyList<DelimiterPair> pairs)
    {
        var openings = pairs.ToDictionary(p => p.Opening.Index);
        var escapes = tokens.OfType<EscapeToken>()
            .Select(t => t.Index)
            .ToHashSet();

        return BuildNodes(text, startIndex, text.Length, openings, escapes);
    }

    private static List<Node> BuildNodes(
        string text,
        int startIndex,
        int endIndex,
        IReadOnlyDictionary<int, DelimiterPair> openings,
        IReadOnlySet<int> escapes)
    {
        var nodes = new List<Node>();

        var index = startIndex;

        var sb = new StringBuilder();

        while(index < endIndex)
        {
            if (escapes.Contains(index))
            {
                sb.Append(text[index + 1]);
                index += 2;
                continue;
            }

            if (openings.TryGetValue(index, out var pair))
            {
                FlushText();

                var children = BuildNodes(
                    text,
                    pair.Opening.Index + pair.Opening.Length,
                    pair.Closing.Index,
                    openings,
                    escapes);

                nodes.Add(pair.Type == DelimiterType.Emphasis
                    ? new EmphasisNode { Children = children }
                    : new StrongNode { Children = children });

                index = pair.Closing.Index + pair.Closing.Length;
                continue;
            }

            sb.Append(text[index++]);
        }

        FlushText();

        return nodes;

        void FlushText()
        {
            if (sb.Length == 0)
            {
                return;
            }

            nodes.Add(new TextNode(sb.ToString()));
            sb.Clear();
        }
    }
}