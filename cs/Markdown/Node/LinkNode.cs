using Markdown.Node.Interfaces;

namespace Markdown.Node;

public record LinkNode(string Url, IReadOnlyList<INode> Children) : INode;
