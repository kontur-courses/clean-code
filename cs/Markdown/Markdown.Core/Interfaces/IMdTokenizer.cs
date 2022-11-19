using Markdown.Core.Entities;

namespace Markdown.Core.Interfaces
{
    public interface IMdTokenizer
    {
        IEnumerable<Token> Tokenize(string mdText);
    }
}