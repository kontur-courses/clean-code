namespace Markdown;

public class SimpleTag : SyntaxNode
{
    private readonly string text;

    public SimpleTag(NodeType type, string text) : base(type, null)
    {
        this.text = text;
    }

    public override string Text => text;
}