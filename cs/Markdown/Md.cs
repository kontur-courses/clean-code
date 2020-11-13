using System;

namespace Markdown
{
    public class Md
    {
   
        public string Render(string text)
        {
            var tokens = TokenReader.ReadTokens(text);
            return Converter.ToHTML(tokens);
        }
    }
}
