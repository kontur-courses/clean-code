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
            return DeleteScreeningUnderscores(html);
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

        private string DeleteScreeningUnderscores(string htmlText)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < htmlText.Length - 1; i++)
            {
                if (!(htmlText[i] == '\\' && htmlText[i + 1] == '_'))
                    builder.Append(htmlText[i]);
            }

            builder.Append(htmlText[htmlText.Length - 1]);
            return builder.ToString();
        }
    }
}