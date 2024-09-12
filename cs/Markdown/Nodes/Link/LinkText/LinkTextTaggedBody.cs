namespace Markdown.Nodes.Link.LinkText;

public class LinkTextTaggedBody : TaggedBodyNode
{
    public LinkTextTaggedBody(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type OpenTagType => typeof(OpenLinkTextNode);
    public override Type CloseTagType => typeof(CloseLinkTextNode);
    public override string TagName { get; }

    public override string ToString() =>
        string.Join("", Children!.InnerElements().Select(child => child.ToString()));
}