using System;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            var parser = new TextParser();
            var tokens = new TokensGenerator(parser).GetTokens(markdown);

            var htmlConverter = new HtmlConverter();
            var htmlText = new HtmlGenerator(htmlConverter).CovertTokensToHtml(tokens);

            return htmlText;
        }
    }
}