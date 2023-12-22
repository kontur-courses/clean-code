using Markdown.Nodes.Link.LinkSource;
using Markdown.Nodes.Link.LinkText;

namespace Markdown.Nodes.Link;

public class LinkNode : TaggedBodyNode
{
    public override Type OpenTagType => typeof(LinkTextTaggedBody);
    public override Type CloseTagType => typeof(LinkSourceTaggedBody);
    public override string TagName => "";
    public override string ToString() => $"<a href=\"{Source}\">{Text}</a>";

    public LinkTextTaggedBody Text { get; private set; }

    public LinkSourceTaggedBody Source { get; private set; }

    public LinkNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
        Text = children!.First() as LinkTextTaggedBody;
        Source = children!.Last() as LinkSourceTaggedBody;
    }
}