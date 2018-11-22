using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenReader
    {
        LinkedList<Token> ReadTokens(string source);
    }
}