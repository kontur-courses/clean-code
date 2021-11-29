using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class EndLineToken: IToken
    {
        public INode ToNode()
        {
            return new StringNode("\n");
        }
    }
}   