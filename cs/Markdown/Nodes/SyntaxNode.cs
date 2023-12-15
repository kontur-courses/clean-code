namespace Markdown;

public abstract class SyntaxNode
{
    public IEnumerable<SyntaxNode> Children;
    public NodeType Type;
    public abstract string Text { get; }

    public SyntaxNode(NodeType type, IEnumerable<SyntaxNode> children)
    {
        Children = children;
        Type = type;
    }
}