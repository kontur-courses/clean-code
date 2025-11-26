namespace Markdown.Core.Parsing.Nodes;

/// <summary>
/// Корень дерева, содержит последовательность блочных узлов (заголовки и абзацы)
/// </summary>
public class DocumentNode : Node
{
    public IList<BlockNode> Children { get; } = new List<BlockNode>();
}