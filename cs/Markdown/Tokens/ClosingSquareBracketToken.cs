using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class ClosingSquareBracketToken: IToken
    {
        public INode ToNode()
        {
            return new StringNode("]");
        }
    }
}