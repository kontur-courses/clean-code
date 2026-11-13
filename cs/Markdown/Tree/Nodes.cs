namespace Markdown.Tree;

public interface INode;

public sealed record TextNode(string Text) : INode;

public sealed record EmNode(IReadOnlyList<INode> Children) : INode;

public sealed record StrongNode(IReadOnlyList<INode> Children) : INode;

public sealed record LinkNode(string Url, IReadOnlyList<INode> Children) : INode;