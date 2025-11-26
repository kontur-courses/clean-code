namespace Markdown.Core.Parsing.Nodes;

/// <summary>
/// Обычный текст без разметки, конечный лист дерева
/// </summary>
public class TextNode(string text) : InlineNode
{
    public string Text { get; } = text;
}