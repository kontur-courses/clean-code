namespace Markdown;

public class BodyTag : SyntaxNode
{
    public BodyTag(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override string Text => string.Join("", Children.Select(child => child.Text));
}