using Markdown.Data.Nodes;

namespace Markdown.TreeTranslator
{
    public interface ITokenTreeTranslator
    {
        string Translate(TokenTreeNode tokenTree);
    }
}