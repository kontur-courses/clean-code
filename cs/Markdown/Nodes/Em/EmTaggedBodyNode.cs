namespace Markdown.Nodes.Em;

public class EmTaggedBodyNode : TaggedBodyNode
{
    public EmTaggedBodyNode(IEnumerable<SyntaxNode> children) : base(children)
    {
    }

    public override Type OpenTagType => typeof(OpenEmNode);
    public override Type CloseTagType => typeof(CloseEmNode);
    public override string Evaluate(IEvaluator evaluator) => evaluator.EvaluateEm(this);
}