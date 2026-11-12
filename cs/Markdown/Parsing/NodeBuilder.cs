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

        foreach (var node in MergeLines(markdown).Select(BuildNode))
        {
            if (node is ListItemNode listItem)
            {
                unorderedList.Add(listItem);
                continue;
            }

            FlushList();

            if (node is not null)
            {
                nodes.Add(node);
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
                Children = unorderedList.ToList()
            });

            unorderedList.Clear();
        }
    }

    private static Node? BuildNode(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }

        if (line.StartsWith(HeaderPrefix))
        {
            return BuildContainerNode<HeaderNode>(line, HeaderPrefix.Length);
        }

        if (line.StartsWith(UnorderedListItemPrefix))
        {
            return BuildContainerNode<ListItemNode>(
                line, UnorderedListItemPrefix.Length);
        }

        return BuildContainerNode<ParagraphNode>(line);
    }

    private static TContainerNode BuildContainerNode<TContainerNode>(string line, int startIndex = 0)
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
            Children = BuildInnerNodes(line, startIndex, line.Length, openings, escapes)
        };
    }

    private static List<Node> BuildInnerNodes(
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
                FlushSb(sb, nodeText => nodes.Add(new TextNode(nodeText)));

                var children = BuildInnerNodes(
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

        FlushSb(sb, nodeText => nodes.Add(new TextNode(nodeText)));

        return nodes;
    }

    private static List<string?> MergeLines(string markdown)
    {
        var lines = new List<string?>();
        var sb = new StringBuilder();

        using var reader = new StringReader(markdown);

        while (reader.ReadLine() is { } line )
        {
            if (string.IsNullOrWhiteSpace(line)
                || line.StartsWith(HeaderPrefix))
            {
                FlushSb(sb, lines.Add);
                lines.Add(line?.Trim());
                continue;
            }

            if (line.StartsWith(UnorderedListItemPrefix))
                FlushSb(sb, lines.Add);

            if (sb.Length > 0)
                sb.Append(' ');

            sb.Append(line.Trim());
        }

        FlushSb(sb, lines.Add);

        return lines;
    }

    private static void FlushSb(StringBuilder sb, Action<string> add)
    {
        if (sb.Length <= 0)
        {
            return;
        }

        add(sb.ToString());
        sb.Clear();
    }
}