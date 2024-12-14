using Markdown.Extensions;
using Markdown.NodeView;
using Markdown.Token;

namespace Markdown.SyntaxRules;

public class TokensInDifferentWordsRule(char[] delimiters) : MdValidationRule
{
    protected override bool CheckNode(INodeView<MdTokenType> currentNode, INodeView<MdTokenType> parentNode)
    {
        return currentNode is { InsideWord: true, Type: MdTokenType.Bold or MdTokenType.Italic }
               && currentNode.Children.Any(
                   n => delimiters.Any(x => n.Text.Contains(x)));
    }
}