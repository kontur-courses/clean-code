namespace Markdown;

public class EmBodyNode : BodyTag
{
    public EmBodyNode(IEnumerable<SyntaxNode> children) : base(NodeType.EmBody, children) { }
}