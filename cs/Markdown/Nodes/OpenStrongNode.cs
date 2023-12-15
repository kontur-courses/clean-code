namespace Markdown;

public class OpenStrongNode : SimpleTag
{
    public OpenStrongNode(string text) : base(NodeType.OpenStrongTag, text) { }
}