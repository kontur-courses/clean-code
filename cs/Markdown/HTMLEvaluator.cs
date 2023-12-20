using Markdown.Link;

namespace Markdown;

public class HTMLEvaluator : IEvaluator
{
    public string Evaluate(RootNode root)
    {
        return string.Join("", root.Children.Select(Evaluate));
    }

    private string Evaluate(SyntaxNode node)
    {
        if (node is LinkNode linkNode)
            return $"<a href=\"{linkNode.Source}\">{linkNode.Text}</a>";
        if (node is TaggedBodyNode)
            return string.Join("", node.Children.Select(Evaluate));

        switch (node)
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
                return node.Text;
            default:
                throw new ArgumentException("Wrong node type");
        }
    }
}