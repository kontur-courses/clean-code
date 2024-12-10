using Markdown.Enums;

namespace Markdown.AstNodes;

public abstract class MarkdownNode
{
    public abstract MarkdownNodeName Type { get; }

    public override bool Equals(object? obj)
    {
        if (this is IMarkdownNodeWithChildren node && obj is IMarkdownNodeWithChildren other)
            return this.GetType() == other.GetType() && node.Children.SequenceEqual(other.Children);
        if (this is TextMarkdownNode valueNode && obj is TextMarkdownNode otherValueNode)
            return valueNode.Content.Equals(otherValueNode.Content);
        return false;
    }
}