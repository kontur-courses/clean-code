using System;

namespace Markdown
{
    public class Markdown
    {
   
        public string Render(string text)
        {
            var tokens = new TokenReader().ReadTokens(text);
            var htmlText = Converter.TokensToHtml(text, tokens);
            return htmlText;
        }
    }
}
