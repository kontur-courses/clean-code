using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawText)
        {
            var tokenizer = new MarkdownTextTokenizer();
            var tokens = tokenizer.GetTokens(rawText);
            return GetHtmlText(tokens);
        }

        private string GetHtmlText(IEnumerable<Token> tokens)
        {
            var text = tokens.Select(TokenConverter.ConvertTokenToHtml);
            return string.Join("", text);
        }
    }
}
