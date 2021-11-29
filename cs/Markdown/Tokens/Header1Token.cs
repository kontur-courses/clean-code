using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class Header1Token: IToken
    {

        public INode ToNode()
        {
            return new FirstHeaderTaggedNode();
        }
    }
}