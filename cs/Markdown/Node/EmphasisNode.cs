using Markdown.Node.Interfaces;
using Markdown.Node.Type;

namespace Markdown.Node;

public record EmphasisNode(EmphasisType EmphasisType, IReadOnlyCollection<INode> Children) : INode;
