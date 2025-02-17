namespace Markdown.treeVisitor;

public interface INode
{
    public bool CanHaveInnerHardNode();
    public List<INode> InnerNode { get; }
    public INode? NextNode { get; set; }
    public string Convert(IVisitor visitor);
}