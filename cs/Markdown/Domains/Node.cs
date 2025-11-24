using System.Text;

namespace Markdown.Domains;

public abstract class Node
{
    public List<Node> Children { get; } = [];

    protected Node(List<Node>? children = null)
    {
        if (children != null)
            Children.AddRange(children);
    }

    public abstract void ConvertToHtml(StringBuilder sb);

    protected void RenderChildren(StringBuilder sb)
    {
        foreach (var child in Children)
            child.ConvertToHtml(sb);
    }
}