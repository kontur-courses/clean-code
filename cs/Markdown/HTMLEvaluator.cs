using Markdown.Nodes;
using Markdown.Nodes.Em;
using Markdown.Nodes.Header;
using Markdown.Nodes.Link;
using Markdown.Nodes.Strong;

namespace Markdown;

public class HtmlEvaluator : IEvaluator
{
    public string EvaluateRoot(RootNode root)
    {
        if (!root.Children!.Any())
            return "";
        return ChildrenEvaluation(root.Children!);
    }

    public string EvaluateLink(LinkNode linkNode) =>
        $"<a href=\"{ChildrenEvaluation(linkNode.Source.Children!)}\">{ChildrenEvaluation(linkNode.Text.Children!)}</a>";

    public string EvaluateString(TextNode textNode) => textNode.Text;

    public string EvaluateStrong(StrongTaggedBodyNode strongTaggedBodyNode) =>
        BaseEvaluation(strongTaggedBodyNode, "strong");

    public string EvaluateHeader(HeaderTaggedBodyNode headerTaggedBodyNode) =>
        BaseEvaluation(headerTaggedBodyNode, "h1");

    public string EvaluateEm(EmTaggedBodyNode emTaggedBodyNode) => BaseEvaluation(emTaggedBodyNode, "em");

    private string ChildrenEvaluation(IEnumerable<SyntaxNode> children)
    {
        return string.Join("", children.Select(child => child.Evaluate(this)));
    }

    private string BaseEvaluation(TaggedBodyNode node, string name)
    {
        if (node.Children == null)
            return node.Evaluate(this);
        return $"<{name}>{ChildrenEvaluation(node.Children)}</{name}>";
    }
}