namespace Markdown.treeVisitor;

public class BoldNode : INode
{
    public bool CanHaveInnerHardNode() => true;
    public List<INode> InnerNode { get;  } = [];
    public INode? NextNode { get; set; }

    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }
}