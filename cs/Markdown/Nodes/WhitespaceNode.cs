namespace Markdown;

public class WhitespaceNode : SimpleTag
{
    public WhitespaceNode(string text) : base(NodeType.WhitespaceNode, text)
    {
    }
}