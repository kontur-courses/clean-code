using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class OpeningSquareBracketToken: IToken
    {

        public INode ToNode()
        {
            return new LinkNode();
        }
    }
}