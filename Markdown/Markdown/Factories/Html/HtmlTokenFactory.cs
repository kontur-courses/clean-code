using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Factories.Html
{
    public class HtmlTokenFactory : ITokenFactory<HtmlToken>
    {
        public HtmlToken NewToken(TokenType type, string value)
        {
            return new HtmlToken(type, value);
        }
    }
}