namespace Markdown.Nodes.Link.LinkSource;

public class LinkSourceTaggedBody : TaggedBodyNode
{
    public LinkSourceTaggedBody(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type OpenTagType => typeof(OpenLinkSourceNode);
    public override Type CloseTagType => typeof(CloseLinkSourceNode);
    public override string TagName => "";
    public override string ToString() => string.Join("", Children!.InnerElements().Select(child => child.ToString()));
}