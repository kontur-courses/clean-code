namespace Markdown.Models.Nodes;

public abstract class ContainerNode : Node
{
    public List<Node> Children { get; } = new();
}