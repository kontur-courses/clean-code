namespace Markdown.Core.Parsing.Nodes;

/// <summary>
/// Заголовок, блочный элемент, содержащий набор инлайнов (текст и выделения)
/// </summary>
public class HeadingNode(int level) : BlockNode
{
    public int Level { get; } = level; //сейчас у нас только h1 по условию, позволяет легко расширить в дальнейшем
    public List<InlineNode> Inlines { get; } = [];
}