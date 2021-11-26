using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenParser
    {
        TokenTree[] Parse(IEnumerable<IToken> tokens);
    }
}