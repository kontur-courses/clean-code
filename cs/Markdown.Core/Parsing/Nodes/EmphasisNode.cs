namespace Markdown.Core.Parsing.Nodes;

/// <summary>
/// Курсив (_..._): контейнер инлайнов внутри выделения
/// ОГРАНИЧЕНИЕ: Не может содержать StrongNode (по спецификации)
/// </summary>
public class EmphasisNode : InlineNode
{
    public IList<InlineNode> Inlines { get; } = new List<InlineNode>();
}