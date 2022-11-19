using Markdown.Core.Entities;

namespace Markdown.Core.Interfaces
{
    public interface IMdParser
    {
        IEnumerable<TagNode> Parse(IEnumerable<Token> tokens);
    }
}