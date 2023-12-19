namespace Markdown;

public class HeaderTaggedBodyNode : TaggedBodyNode
{
    public HeaderTaggedBodyNode(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override Type openTagType => typeof(OpenHeaderNode);
    public override Type closeTagType => typeof(CloseHeaderNode);
}