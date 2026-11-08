namespace Markdown.Models.Nodes;

public class TextNode : Node
{
    public string Content { get; }

    public TextNode(string content)
    {
        Content = content;
    }
}