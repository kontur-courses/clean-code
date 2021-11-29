using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class OpeningRoundBracketToken: IToken
    {

        public INode ToNode()
        {
            return new StringNode("(");
        }
    }
}