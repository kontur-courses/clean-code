namespace Markdown.Nodes;

public abstract record Node;

public record Root(IReadOnlyList<BlockNode> Blocks) : Node; // Будем строить дерево нод в процессе парсинга, тогда Root - корень такого дерева.