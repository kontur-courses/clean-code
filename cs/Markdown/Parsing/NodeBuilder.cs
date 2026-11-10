using System.Text;
using Markdown.Domain;

namespace Markdown.Parsing;

internal static class NodeBuilder
{
    private const string HeaderPrefix = "# ";
    private const string UnorderedListItemPrefix = "- ";

    public static IReadOnlyList<Node> Build(string markdown)
    {
        var nodes = new List<Node>();
        var unorderedList = new List<Node>();

        foreach (var line in MergeLines(markdown))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                FlushList();
                continue;
            }

            if (line.StartsWith(HeaderPrefix))
            {
                FlushList();
                nodes.Add(BuildBlock<HeaderNode>(
                    line, HeaderPrefix.Length));
            }
            else if (line.StartsWith(UnorderedListItemPrefix))
            {
                unorderedList.Add(BuildBlock<ListItemNode>(
                    line, UnorderedListItemPrefix.Length));
            }
            else
            {
                FlushList();
                nodes.Add(BuildBlock<ParagraphNode>(line));
            }
        }

        FlushList();
        return nodes;

        void FlushList()
        {
            if (unorderedList.Count == 0)
            {
                return;
            }

            nodes.Add(new UnorderedListNode
            {
                Children = [.. unorderedList]
            });

            unorderedList.Clear();
        }
    }

    private static TContainerNode BuildBlock<TContainerNode>(string line, int startIndex = 0)
        where TContainerNode : ContainerNode, new()
    {
        var tokens = InlineTokenizer.Tokenize(line, startIndex);
        var pairs = DelimiterMatcher.FindPairs(tokens, line);

        var openings = pairs.ToDictionary(p => p.Opening.Index);
        var escapes = tokens.OfType<EscapeToken>()
            .Select(t => t.Index)
            .ToHashSet();

        return new TContainerNode
        {
            Children = BuildNodes(line, startIndex, line.Length, openings, escapes)
        };
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

    private static List<string> MergeLines(string markdown)
    {
        var lines = new List<string>();
        var sb = new StringBuilder();

        using var reader = new StringReader(markdown);

        while (reader.ReadLine() is { } line )
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                FlushSb();

                lines.Add(string.Empty);
                continue;
            }

            if (line.StartsWith(HeaderPrefix)
                || line.StartsWith(UnorderedListItemPrefix))
            {
                FlushSb();
            }

            if (line.StartsWith(HeaderPrefix))
            {
                lines.Add(line.Trim());
                continue;
            }

            if (sb.Length > 0)
            {
                sb.Append(' ');
            }

            sb.Append(line.Trim());
        }

        FlushSb();

        return lines;

        void FlushSb()
        {
            if (sb.Length <= 0)
            {
                return;
            }

            lines.Add(sb.ToString());
            sb.Clear();
        }
    }
}