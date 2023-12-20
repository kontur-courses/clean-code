namespace Markdown;

public class StrongTaggedBodyNode : TaggedBodyNode
{
    public StrongTaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type openTagType => typeof(OpenStrongNode);
    public override Type closeTagType => typeof(CloseStrongNode);
}