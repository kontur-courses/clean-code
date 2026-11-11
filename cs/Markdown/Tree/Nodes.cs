namespace Markdown.Tree;

public interface INode;

public sealed class TextNode : INode
{
    public TextNode(string text)
    {
        Text = text;
    }

    public string Text { get; }
}

public sealed class EmNode : INode
{
    public EmNode(IReadOnlyList<INode> children)
    {
        Children = children;
    }

    public IReadOnlyList<INode> Children { get; }
}

public sealed class StrongNode : INode
{
    public StrongNode(IReadOnlyList<INode> children)
    {
        Children = children;
    }

    public IReadOnlyList<INode> Children { get; }
}

public sealed class LinkNode : INode
{
    public LinkNode(string url, IReadOnlyList<INode> children)
    {
        Url = url;
        Children = children;
    }

    public string Url { get; }

    public IReadOnlyList<INode> Children { get; }
}