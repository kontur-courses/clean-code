namespace Markdown.Primitives;

public class TagNode
{
    public Tag Tag { get; init; }
    public List<TagNode> Childs { get; init; }
}