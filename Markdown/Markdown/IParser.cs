using Markdown.ASTNodes;

namespace Markdown
{
    public interface IParser
    {
        IElement Parse();
    }
}
