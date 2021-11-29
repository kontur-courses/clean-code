using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class SpaceToken: IToken
    {
        public INode ToNode()
        {
            return new StringNode(" ");
        }
    }
}