using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenizer
    {
        IEnumerable<IToken> ParseText(string source);
    }
}