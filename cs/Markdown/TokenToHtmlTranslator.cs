using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenToHtmlTranslator : ITokenTranslator
    {
        private static readonly Dictionary<TokenType, string> tokenToHtml = new Dictionary<TokenType, string>()
        {
            {TokenType.Heading, "h1"},
            {TokenType.Bold, "strong"},
            {TokenType.Italics, "em"}
        };

        public string Translate(IEnumerable<Token> tokens)
        {
            var htmlMarkup = new StringBuilder();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Content:
                        htmlMarkup.Append(token.Value);
                        break;
                    case TokenType.Heading:
                        htmlMarkup.Append(token.IsOpening
                            ? $"<{tokenToHtml[token.Type]}>"
                            : $"</{tokenToHtml[token.Type]}>");
                        break;
                    case TokenType.Bold:
                    case TokenType.Italics:
                        if (token.ShouldBeSkipped)
                            htmlMarkup.Append(token.Value);
                        else
                            htmlMarkup.Append(token.IsOpening
                                ? $"<{tokenToHtml[token.Type]}>"
                                : $"</{tokenToHtml[token.Type]}>");
                        break;
                }
            }
            return htmlMarkup.ToString();
        }
    }
}
