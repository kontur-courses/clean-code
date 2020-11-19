using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface IConverter
    {
        string ConvertTokens(IEnumerable<IToken> tokens);
    }
}