namespace Markdown.LinkSource;

public class LinkSourceTaggedBody : TaggedBodyNode
{
    public LinkSourceTaggedBody(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override Type openTagType => typeof(OpenLinkSourceNode);
    public override Type closeTagType => typeof(CloseLinkSourceNode);
}