namespace Markdown;

public abstract class SyntaxNode
{
    public IEnumerable<SyntaxNode> Children;
    public abstract string Text { get; }

    public SyntaxNode(IEnumerable<SyntaxNode> children)
    {
        Children = children;
    }
}