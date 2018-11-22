using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenSelector
    {
        IEnumerable<Token> SelectTokens(LinkedList<Token> tokens);
    }
}