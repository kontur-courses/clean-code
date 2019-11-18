using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.RenderUtilities;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawText)
        {
            var tokenDescription = MarkdownTokenUtilities.GetMarkdownTokenDescriptions();
            List<Token> tokens = new TokenReader(tokenDescription).TokenizeText(rawText);

            return MarkdownRenderUtilities.GetMarkdownRenderer().RenderText(tokens);
        }
    }
}