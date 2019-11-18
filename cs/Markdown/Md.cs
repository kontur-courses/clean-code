using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawText)
        {
            var tokenDescription = MarkdownTokenUtilities.GetMarkdownTokenDescriptions();
            List<Token> tokens = new TokenReader(tokenDescription).TokenizeText(rawText);

            return RenderFromTokens(tokens, tokenDescription);
        }

        public string RenderFromTokens(IEnumerable<Token> tokens,
            IEnumerable<TokenDescription> tokenDescription)
        {
            var resultString = new StringBuilder();
            throw new NotImplementedException();
        }
    }
}