using Markdown.Link;

namespace Markdown;

public class HTMLEvaluator : IEvaluator
{
    public string Evaluate(SyntaxNode root)
    {
        if (root is RootNode)
            return string.Join("", root.Children.Select(child => Evaluate(child)));
        if (root is LinkNode linkNode)
            return $"<a href=\"{linkNode.Source}\">{linkNode.Text}</a>";
        if (root is TaggedBodyNode)
            return string.Join("", root.Children.Select(child => Evaluate(child)));

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