namespace Markdown.Nodes;

public abstract record BlockNode : Node;
//BlockNode - такая нода, тип которой задаётся в начале абзаца, определяет его тип(обычный абзац/заголовок/что-то ещё в будущем, что требует абзаца, напр список.
internal record TextBlock(IReadOnlyList<TextNode> Children) : BlockNode;

internal record HeaderBlock(IReadOnlyList<TextNode> Children) : BlockNode;

//internal record UnorderedListItemBlock(IReadOnlyList<TextNode> Children) : BlockNode;
