using Markdown.Nodes;

namespace Markdown;

public static class Extensions
{
    public static IEnumerable<SyntaxNode> TextifyInnerTags(this IEnumerable<SyntaxNode> nodes)
    {
        if (nodes.Count() < 2)
            throw new ArgumentException("Method takes at least two node");
        yield return nodes.First();
        foreach (var node in nodes.Skip(1).Take(nodes.Count() - 2).TextifyTags())
            yield return node;
        yield return nodes.Last();
    }

    public static IEnumerable<SyntaxNode>? TextifyTags(this IEnumerable<SyntaxNode> nodes)
    {
        foreach (var node in nodes)
        {
            if (node is TextNode || node is TaggedBodyNode)
                yield return node;
            else
                yield return new TextNode(node.Text);
        }
    }

    public static bool IsOnlyDigitsInInnerTextTags(this IEnumerable<SyntaxNode> nodes)
    {
        if (nodes.Count() < 3)
            throw new ArgumentException("Method takes at least three node");
        return nodes
            .Skip(1)
            .Take(nodes.Count() - 2)
            .Where(node => node is TextNode)
            .SelectMany(node => node.Text)
            .All(char.IsDigit);
    }
}