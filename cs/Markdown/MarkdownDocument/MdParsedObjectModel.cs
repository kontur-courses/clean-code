namespace Markdown.MarkdownDocument;

public class MdParsedObjectModel
{
    public List<IDocumentNode> Nodes { get; private set; }

    public MdParsedObjectModel()
    {
        Nodes = new List<IDocumentNode>();
    }

    public void AddNode(IDocumentNode node)
    {
        Nodes.Add(node);
    }
}