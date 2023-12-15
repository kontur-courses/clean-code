namespace Markdown;

public class CloseStrongNode : SimpleTag
{
    public CloseStrongNode(string text) : base(NodeType.CloseStrongTag, text) { }
}