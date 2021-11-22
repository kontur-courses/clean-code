using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Converter
{
    public interface IConverter
    {
        string ConvertTokensInText(List<IToken> tokens, string text);
    }
}