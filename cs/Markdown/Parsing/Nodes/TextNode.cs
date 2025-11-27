namespace Markdown.Parsing.Nodes;

public class TextNode : MarkdownNode
{
    public string Content { get; set; }
    
    public override string ToHtml() => Content;
}