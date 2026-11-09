using Markdown.Node.Interfaces;
using Markdown.Node.Type;

namespace Markdown.Node;

public record BlockNode(BlockType BlockType, IReadOnlyCollection<INode> Children) : INode;