using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Factories.Html
{
    public class HtmlTokenFactory : ITokenFactory<HtmlToken>
    {
        public HtmlToken NewToken(TokenType type, string value, IEnumerable<HtmlToken> childTokens)
        {
            return new HtmlToken(type, value, childTokens);
        }
    }
}