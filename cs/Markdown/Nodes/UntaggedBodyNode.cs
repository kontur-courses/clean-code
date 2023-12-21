namespace Markdown.Nodes;

public class UntaggedBodyNode : SyntaxNode
{
    public UntaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override string Text => string.Join("", Children!);

    public override string Evaluate(IEvaluator evaluator) => "";
}