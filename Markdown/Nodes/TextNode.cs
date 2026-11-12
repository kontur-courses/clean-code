namespace Markdown.Nodes;

public abstract record TextNode : Node;
//TextNode - такая нода, которая лежит внутри абзаца(просто текст, жирный/курсивный/зачёркнуто-перечёркнутый текст, ссылки, изображения и тд)

internal record Text(string Value) : TextNode;
internal record Strong(IReadOnlyList<TextNode> Children) : TextNode;
internal record Emphasis(string Value) : TextNode;
