using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public interface IConverter
    {
        string Convert(IEnumerable<Token> tokens, string text);
    }
}