namespace Markdown.Nodes;

public abstract record BlockNode : Node;
//BlockNode - такая нода, тип которой задаётся в начале абзаца, определяет его тип(обычный абзац/заголовок/что-то ещё в будущем, что требует абзаца, напр список. Здесь я сознательно нарушаю YAGNI (:  )
internal record TextBlock(TextBlockKind Kind, IReadOnlyList<TextNode> Children) : BlockNode;
public enum TextBlockKind
{
    Paragraph,
    Heading,
}
