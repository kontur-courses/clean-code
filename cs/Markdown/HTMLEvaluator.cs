namespace Markdown;

public class HTMLEvaluator : IEvaluator
{
    public string Evaluate(SyntaxNode root)
    {
        if (root is BodyTag)
            return string.Join("", root.Children.Select(child => Evaluate(child)));

        if (root is not SimpleTag) throw new ArgumentException("Wrong node type");

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
            case TextNode:
            case WhitespaceNode:
                return root.Text;
            default:
                throw new ArgumentException("Wrong node type");
        }
    }
}