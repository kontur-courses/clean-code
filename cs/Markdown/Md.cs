using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string markDownParagraph)
        {
            var parser = new MdParser();
            var tokens = parser.GetTokens(markDownParagraph);
            var converter = new MdConverter(tokens, markDownParagraph);
            var htmlTokens = converter.GetHtmlTokens();
            return string.Join("", htmlTokens);
        }
    }
}