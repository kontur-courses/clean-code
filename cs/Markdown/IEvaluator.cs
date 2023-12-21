using Markdown.Nodes;
using Markdown.Nodes.Em;
using Markdown.Nodes.Header;
using Markdown.Nodes.Link;
using Markdown.Nodes.Strong;

namespace Markdown;

public interface IEvaluator
{
    string EvaluateRoot(RootNode root);
    string EvaluateLink(LinkNode linkNode);
    string EvaluateString(TextNode textNode);
    string EvaluateStrong(StrongTaggedBodyNode strongTaggedBodyNode);
    string EvaluateHeader(HeaderTaggedBodyNode headerTaggedBodyNode);
    string EvaluateEm(EmTaggedBodyNode emTaggedBodyNode);
}