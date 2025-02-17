namespace Markdown.treeVisitor;

public class LineNode : INode
{
    public INode? NextNode { get; set; }
    public bool CanHaveInnerHardNode() => false;

    public List<INode> InnerNode { get; set; } = [];

    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }

    public void SetNextNode(INode node)
    {
        NextNode = node;
    }
}