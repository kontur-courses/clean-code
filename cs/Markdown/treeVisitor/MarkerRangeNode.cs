namespace Markdown.treeVisitor;

public class MarkerRangeNode : INode
{
    public bool CanHaveInnerHardNode()
    {
        throw new NotImplementedException();
    }

    public List<INode> InnerNode { get; } = [];
    public INode? NextNode { get; set; }
    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }
}