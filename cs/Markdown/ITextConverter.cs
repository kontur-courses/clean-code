using Markdown.Tokens;
using System.Collections.Generic;

namespace Markdown
{
    internal interface ITextConverter
    {
        string Convert(List<Token> tokens);
    }

    internal class TextConverter
    {
        public static ITextConverter HTMLConverter() => new HTMLConverter();
    }
}
