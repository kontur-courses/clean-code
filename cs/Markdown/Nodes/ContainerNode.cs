using Markdown.Nodes.HtmlTags;
using System.Text;

namespace Markdown.Nodes;

public class ContainerNode : INode
{
    public HtmlTag Tag { get; }
    public List<INode> Children { get; }

    public ContainerNode(HtmlTag tag, List<INode> children)
    {
        Tag = tag;
        Children = children;
    }

    public void Render(StringBuilder sb)
    {
        var tagName = Tag.HtmlToString();
        sb.Append('<').Append(tagName).Append('>');

        foreach (var child in Children)
        {
            child.Render(sb);
        }

        sb.Append("</").Append(tagName).Append('>');
    }
}