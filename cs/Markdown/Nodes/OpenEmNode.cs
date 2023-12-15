namespace Markdown;

public class OpenEmNode : SimpleTag
{
    public OpenEmNode(string text) : base(NodeType.OpenEmTag, text) { }
}