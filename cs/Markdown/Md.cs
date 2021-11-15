using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var tokens = MdParser.ParseTokens(text);
            var result = MdParser.ConvertToHtml(tokens);

            return result;
        }
    }
}
