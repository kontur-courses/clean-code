namespace Markdown.LinkText;

public class LinkTextTaggedBody : TaggedBodyNode
{
    public LinkTextTaggedBody(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type openTagType => typeof(OpenLinkTextNode);
    public override Type closeTagType => typeof(CloseLinkTextNode);
}