using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class BoldToken: IToken
    {
        public INode ToNode()
        {
            return new StrongTaggedNode();
        }
    }
}