namespace Markdown;

public abstract class Node
{
    public List<Node> Children { get; set; } = new List<Node>();

}