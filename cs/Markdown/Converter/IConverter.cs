using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Converter
{
    public interface IConverter
    {
        public string ConvertTokens(IEnumerable<IToken> tokens);
    }
}