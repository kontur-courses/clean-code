using Markdown.NodeView;
using Markdown.Token;

namespace Markdown.SyntaxRules;

public class NestingRule : MdValidationRule
{
    protected override bool CheckNode(INodeView<MdTokenType> currentNode, INodeView<MdTokenType> parentNode)
    {
        return currentNode.Type == MdTokenType.Bold && parentNode.Type == MdTokenType.Italic;
    }
}