using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class ClosingRoundBracketToken: IToken
    {
        public INode ToNode()
        {
            return new StringNode(")");
        }
    }
}