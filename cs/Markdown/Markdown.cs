using System;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            var tokens = new TextParser().GetTokens(markdown);
            var htmlText = new HtmlConverter().ConvertTokens(tokens);

            return htmlText;
        }
    }
}