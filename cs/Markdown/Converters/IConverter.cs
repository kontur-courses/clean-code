using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface IConverter
    {
        string ConvertTokens(IEnumerable<Token> tokens);
    }
}