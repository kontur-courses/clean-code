using Markdown.Node.Interfaces;

namespace Markdown.Node;

public record TextNode(string Text) : INode;