using System.Collections.Generic;

namespace Markdown
{
    internal interface IConverter
    {
        public string Convert(string text, IEnumerable<Token> tokens, TextInfo textInfo);
    }
}