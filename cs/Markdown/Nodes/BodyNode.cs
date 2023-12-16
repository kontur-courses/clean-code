namespace Markdown;

public class BodyNode : SyntaxNode
{
    public BodyNode(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override string Text => string.Join("", Children.Select(child => child.Text));
}