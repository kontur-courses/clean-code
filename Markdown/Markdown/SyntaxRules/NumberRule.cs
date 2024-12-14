using Markdown.Extensions;
using Markdown.NodeView;
using Markdown.Token;

namespace Markdown.SyntaxRules;

public class NumberRule : MdValidationRule
{
    protected override bool CheckNode(INodeView<MdTokenType> currentNode, INodeView<MdTokenType> parentNode)
    {
        return currentNode is { InsideWord: true, Type: MdTokenType.Bold or MdTokenType.Italic }
               && currentNode.Children.Any(
                   n => n.Text.ContainsNumber());
    }
}