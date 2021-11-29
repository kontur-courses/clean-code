using Markdown.Nodes;

namespace Markdown.Tokens
{
    public class ItalicToken: IToken
    {

        public INode ToNode()
        {
            return new EmphasizedTaggedNode();
        }
    }
}