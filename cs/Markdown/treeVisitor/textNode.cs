namespace Markdown.treeVisitor;

public class TextNode : INode
{
    public string Text { get; set; }

    public TextNode(string text)
    {
        Text = text;
    }

    public bool CanHaveInnerHardNode() => false;

    public List<INode> InnerNode { get; set; } = [];
    public INode NextNode { get; set; }

    public string Convert(IVisitor visitor)
    {
        return visitor.Convert(this);
    }
}