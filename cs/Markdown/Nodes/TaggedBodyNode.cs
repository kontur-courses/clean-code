namespace Markdown;

public abstract class TaggedBodyNode : SyntaxNode
{
    public abstract Type openTagType { get; }
    public abstract Type closeTagType { get; }

    public TaggedBodyNode(IEnumerable<SyntaxNode> _children) : base(_children)
    {
        var children = _children.ToArray();
        if (children.Count() < 2)
            throw new ArgumentException("Tagged body node must have at least 2 nodes");
        var f = children.First();
        if (children.First().GetType() != openTagType)
            throw new ArgumentException(
                $"Open child tag must have type {openTagType} but have {children.First().GetType()}");
        if (children.Last().GetType() != closeTagType)
            throw new ArgumentException(
                $"Close child tag must have type {closeTagType} but have {children.Last().GetType()}");
    }

    public override string Text => string.Join("", Children.Select(child => child.Text));

    public string InnerText
    {
        get
        {
            if (Children.Count() == 2)
                return "";
            return string.Join("", Children
                .Skip(1)
                .Take(Children.Count() - 2)
                .Select(child => child.Text));
        }
    }
}