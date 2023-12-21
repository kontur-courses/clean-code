namespace Markdown.Nodes;

public abstract class SyntaxNode
{
    public readonly IEnumerable<SyntaxNode>? Children;
    public abstract string Text { get; }

    public SyntaxNode(IEnumerable<SyntaxNode>? children)
    {
        Children = children;
    }

    public abstract string Evaluate(IEvaluator evaluator);
}