using System.Collections.Generic;

namespace Markdown
{
    public abstract class Converter
    {
        public abstract string GetHtml(List<Token> tokens, string text);
    }
}