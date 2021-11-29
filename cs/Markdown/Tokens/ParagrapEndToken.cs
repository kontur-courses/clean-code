using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class ParagraphEndToken: IToken
    {
        public INode ToNode()
        {
            return new StringNode("\n\n");
        }
    }
}