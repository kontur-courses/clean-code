using System.Collections.Generic;
using Markdown.Tokenization;

namespace Markdown.Converters
{
    public interface IConverter
    {
        string Convert(IEnumerable<Token> tokens, string text);
    }
}