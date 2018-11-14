using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenType[] fieldTypes = { new ItalicField() };

        public string Render(string mdText)
        {
            var parser = new Parser(mdText, fieldTypes);
            var tokens = parser.GetTokens();
            var html = TokensToHtml(tokens, mdText);
            return html;
        }

        private string TokensToHtml(IEnumerable<Token> tokens, string mdText)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                builder.Append(token.ToHtml(mdText));
            }

            return builder.ToString();
        }
    }
}