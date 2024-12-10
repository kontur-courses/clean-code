using Markdown.Enums;

namespace Markdown.AstNodes;

public class TextMarkdownNode(string content) : MarkdownNode
{
    public override MarkdownNodeName Type => MarkdownNodeName.Text;
    public string Content => content;
}