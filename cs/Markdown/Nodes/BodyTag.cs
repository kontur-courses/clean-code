namespace Markdown;

public class BodyTag : SyntaxNode
{
    public BodyTag(NodeType type, IEnumerable<SyntaxNode> children) : base(type, children) { }

    public override string Text => string.Join("", Children.Select(child => child.Text));
}