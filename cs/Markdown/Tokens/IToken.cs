using Markdown.Nodes;

namespace Markdown.Tokens
{
    public interface IToken
    {
        INode ToNode();
    }
}