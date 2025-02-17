namespace Markdown.treeVisitor;

public class MarkerNode : INode
{
    public bool CanHaveInnerHardNode()
    {
        throw new NotImplementedException();
    }

    public List<INode> InnerNode { get; } = new List<INode>();
    public INode? NextNode { get; set; }
    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }
}