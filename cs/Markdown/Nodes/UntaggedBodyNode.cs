namespace Markdown.Nodes;

public abstract class UntaggedBodyNode : SyntaxNode
{
    public UntaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }
}