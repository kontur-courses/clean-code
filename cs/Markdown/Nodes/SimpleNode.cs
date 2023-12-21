namespace Markdown.Nodes;

public class SimpleNode : SyntaxNode
{
    private readonly string text;

    public SimpleNode(string text) : base(null)
    {
        this.text = text;
    }

    public override string Text => text;
    public override string Evaluate(IEvaluator evaluator) => "";
}