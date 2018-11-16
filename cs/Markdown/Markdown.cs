using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string paragraph)
        {
            var tokens = new TokenFinder().GetTokensWithPositions(paragraph);
            return new Md2HtmlTranslator().TranslateMdToHtml(paragraph, tokens);
        }
    }
}
