namespace Markdown.Tree;

public interface INode;

public record TextNode(string Text) : INode;

public record EmNode(IReadOnlyList<INode> Children) : INode;

public record StrongNode(IReadOnlyList<INode> Children) : INode;

public record LinkNode(string Url, IReadOnlyList<INode> Children) : INode;