namespace Markdown.Nodes;

public abstract class TaggedBodyNode : SyntaxNode
{
    public abstract Type OpenTagType { get; }
    public abstract Type CloseTagType { get; }

    public TaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
        if (children!.Count() < 2)
            throw new ArgumentException("Tagged body node must have at least 2 nodes");
        if (children.First().GetType() != OpenTagType)
            throw new ArgumentException(
                $"Open child tag must have type {OpenTagType} but have {children.First().GetType()}");
        if (children.Last().GetType() != CloseTagType)
            throw new ArgumentException(
                $"Close child tag must have type {CloseTagType} but have {children.Last().GetType()}");
    }

    public override string Text => string.Join("", Children!.Select(child => child.Text));

    public string InnerText
    {
        get
        {
            if (Children!.Count() == 2)
                return "";
            return string.Join("", Children!
                .Skip(1)
                .Take(Children!.Count() - 2)
                .Select(child => child.Text));
        }
    }
}