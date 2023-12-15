namespace Markdown;

public class HTMLEvaluator : IEvaluator
{
    public string Evaluate(SyntaxNode root)
    {
        if (root is BodyTag)
        {
            return string.Join("", root.Children.Select(child => Evaluate(child)));
        }
        if (root is SimpleTag)
        {
            switch (root.Type)
            {
                case NodeType.OpenEmTag:
                    return "<em>";
                case NodeType.CloseEmTag:
                    return "</em>";
                case NodeType.OpenStrongTag:
                    return "<strong>";
                case NodeType.CloseStrongTag:
                    return "</strong>";
                case NodeType.WhitespaceNode:
                case NodeType.TextNode:
                    return root.Text;
            }
        }

        throw new ArgumentException("Wrong node type");
    }
}