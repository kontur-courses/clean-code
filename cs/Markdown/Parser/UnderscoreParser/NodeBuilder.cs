using Markdown.Parsing;

namespace Markdown;

public static class NodeBuilder
{
    public static void BuildNode(Underscore opener, int closingPlaceholderIndex, List<INode> nodes)
    {
        var childCount = closingPlaceholderIndex - opener.ContentStartIndex;
        if (childCount <= 0)
            return;

        var innerNodes = nodes.GetRange(opener.ContentStartIndex, childCount);
        nodes.RemoveRange(opener.ContentStartIndex, childCount + 1);

        INode replacement = opener.IsStrong
            ? new StrongNode(innerNodes)
            : new EmphasisNode(innerNodes);

        nodes[opener.NodeIndex] = replacement;
    }
}