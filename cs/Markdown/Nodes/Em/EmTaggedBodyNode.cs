namespace Markdown;

public class EmTaggedBodyNode : TaggedBodyNode
{
    public EmTaggedBodyNode(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override Type openTagType => typeof(OpenEmNode);
    public override Type closeTagType => typeof(CloseEmNode);
}