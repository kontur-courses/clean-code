using System.Collections.Generic;
using Markdown.RenderUtilities;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawText)
        {
            var tokenDescription = MarkdownTokenUtilities.GetMarkdownTokenDescriptions();
            var tokens = new TokenReader(tokenDescription).TokenizeText(rawText);

            return MarkdownRenderUtilities.GetMarkdownRenderer().RenderText(tokens);
        }
    }
}