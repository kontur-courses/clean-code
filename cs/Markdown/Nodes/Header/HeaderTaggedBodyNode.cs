namespace Markdown.Nodes.Header;

public class HeaderTaggedBodyNode : TaggedBodyNode
{
    public HeaderTaggedBodyNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override Type OpenTagType => typeof(OpenHeaderNode);
    public override Type CloseTagType => typeof(CloseHeaderNode);
    public override string Evaluate(IEvaluator evaluator) => evaluator.EvaluateHeader(this);
}