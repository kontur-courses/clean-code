namespace Markdown;

public class StrongBodyNode : BodyTag
{
    public StrongBodyNode(IEnumerable<SyntaxNode> children) : base(NodeType.StrongBody, children) { }
}