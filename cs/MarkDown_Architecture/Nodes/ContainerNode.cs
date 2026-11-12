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
        throw new NotImplementedException();
    }
}