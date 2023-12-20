using Markdown.LinkSource;
using Markdown.LinkText;

namespace Markdown.Link;

public class LinkNode : TaggedBodyNode
{
    public override Type openTagType => typeof(LinkTextTaggedBody);
    public override Type closeTagType => typeof(LinkSourceTaggedBody);
    public string Text { get; private set; }
    public string Source { get; private set; }

    public LinkNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
        Text = (children.First() as LinkTextTaggedBody).InnerText;
        Source = (children.Last() as LinkSourceTaggedBody).InnerText;
    }
}