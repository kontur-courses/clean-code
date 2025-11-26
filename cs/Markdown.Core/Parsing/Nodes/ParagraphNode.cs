namespace Markdown.Core.Parsing.Nodes;

/// <summary>
/// Абзац, блочный элемент, содержащий набор инлайнов (текст и выделения)
/// </summary>
public class ParagraphNode : BlockNode
{ 
    public IList<InlineNode> Inlines { get; } = new List<InlineNode>();
}