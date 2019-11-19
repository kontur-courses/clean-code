using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string markDownParagraph, Parser parser, Converter converter)
        {
            var tokens = parser.GetTokens(markDownParagraph);
            return converter.GetHtml(tokens, markDownParagraph);
        }
    }
}