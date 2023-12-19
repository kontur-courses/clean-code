namespace Markdown;

public class RootNode : UntaggedBodyNode
{
    public RootNode(IEnumerable<SyntaxNode> children) : base(children)
    {
    }
}