namespace Markdown.Nodes;

public class RootNode : UntaggedBodyNode
{
    public RootNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }
}