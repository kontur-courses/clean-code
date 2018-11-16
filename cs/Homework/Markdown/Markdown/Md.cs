using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string mdParagraph)
        {
            var possibleTokens = TokenLogic.GetPossibleTokens(mdParagraph);

            var validTokens = possibleTokens.GetValidTokens();
            validTokens.RemoveStrongInEmphasis();

            return GetHtmlParagraph(mdParagraph, validTokens);
        }

        private static string GetHtmlParagraph(string mdParagraph, List<Token> tokens)
        {
            tokens = tokens
                .OrderBy(token => token.Position)
                .ToList();
            var htmlParagraph = new StringBuilder();
            var previousPosition = 0;

            foreach (var token in tokens)
            {
                htmlParagraph.Append(mdParagraph.Substring(previousPosition, token.Position - previousPosition));
                htmlParagraph.Append(token.GetHTMLTag());
                previousPosition = token.Position + token.Tag.MD.Length;
            }

            htmlParagraph.Append(mdParagraph.Substring(previousPosition, mdParagraph.Length -  previousPosition));

            return htmlParagraph.ToString();
        }
    }
}
