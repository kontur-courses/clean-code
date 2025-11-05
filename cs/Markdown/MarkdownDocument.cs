using Markdown.Nodes.Interfaces;
using Markdown.Nodes.Internal;

namespace Markdown;

public class MarkdownDocument
{
    private DocumentNode root;
    private MarkdownNode currentNode;

    public MarkdownDocument()
    {
        root = new DocumentNode(null, "");
    }

    public void AddNode(MarkdownNode node)
    {
        
    }

    public string ToHtml()
    {
        throw new NotImplementedException();
    }
}