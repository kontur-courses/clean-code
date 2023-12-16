namespace Markdown;

public class SimpleTag : SyntaxNode
{
    private readonly string text;

    public SimpleTag(string text) : base(null)
    {
        this.text = text;
    }

    public override string Text => text;
}