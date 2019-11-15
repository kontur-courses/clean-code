using System.Collections.Generic;

namespace Markdown
{
    public interface IReadingState
    {
        IReadingState ProcessSymbol(char symbol);
        IEnumerable<Token> GetContentTokens();
    }
}
