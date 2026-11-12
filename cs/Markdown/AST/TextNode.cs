namespace Markdown.AST;

public sealed class TextNode : InlineNode
{
    public string Text { get; init; } = "";
}
