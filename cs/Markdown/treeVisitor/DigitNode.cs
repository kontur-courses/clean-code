namespace Markdown.treeVisitor;

public class DigitNode : INode // в ней особой необходимости нету
{
    public int Digit { get; set; }
    public bool CanHaveInnerHardNode() => false;
    public List<INode> InnerNode { get; set; }
    public INode? NextNode { get; set; }
    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }
}