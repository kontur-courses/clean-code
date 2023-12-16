namespace Markdown;

public class HTMLEvaluator : IEvaluator
{
    public string Evaluate(SyntaxNode root)
    {
        if (root is BodyNode)
            return string.Join("", root.Children.Select(child => Evaluate(child)));

        if (root is not SimpleNode) throw new ArgumentException("Wrong node type");

        switch (root)
        {
            case OpenEmNode:
                return "<em>";
            case CloseEmNode:
                return "</em>";
            case OpenStrongNode:
                return "<strong>";
            case CloseStrongNode:
                return "</strong>";
            case OpenHeaderNode:
                return "<h1>";
            case CloseHeaderNode:
                return "</h1>";
            case TextNode:
                return root.Text;
            default:
                throw new ArgumentException("Wrong node type");
        }
    }
}