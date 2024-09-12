namespace Markdown.Nodes;

public class SimpleNode : SyntaxNode
{
    public SimpleNode(string text) : base(null)
    {
        Text = text;
    }

    public override string Text { get; }
}