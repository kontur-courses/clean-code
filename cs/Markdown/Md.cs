using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public readonly Dictionary<string, Styles> MarkdownStyles = new Dictionary<string, Styles>
        {
            {"_", Styles.Italic}, {"__", Styles.Bold}, {"#", Styles.Title}
        };
        public string Render(string markdownText)
        {
            var tokens = TextAnalyzer.GetTokens(markdownText, MarkdownStyles.Keys.ToArray());
            var htmlText = markdownText;
            foreach (var tokenToTag in GetTokensToTags(tokens))
            {
                var markdownSymbol = markdownText[tokenToTag.StartPosition].ToString();
                htmlText = HtmlCreator.AddHtmlTagToText(htmlText, tokenToTag, MarkdownStyles[markdownSymbol]);
            }
            return htmlText;
        }

        public List<Token> GetTokensToTags(IEnumerable<Token> allTokens)
        {
            throw new NotImplementedException();
        }
    }
}
