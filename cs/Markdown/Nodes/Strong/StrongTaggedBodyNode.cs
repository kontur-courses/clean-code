namespace Markdown.Nodes.Strong;

public class StrongTaggedBodyNode : TaggedBodyNode
{
    public StrongTaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type OpenTagType => typeof(OpenStrongNode);
    public override Type CloseTagType => typeof(CloseStrongNode);
    public override string TagName => "strong";
}