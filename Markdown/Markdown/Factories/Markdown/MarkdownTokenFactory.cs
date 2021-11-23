using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Factories.Markdown
{
    public class MarkdownTokenFactory : ITokenFactory<MarkdownToken>
    {
        public MarkdownToken NewToken(TokenType type, string value)
        {
            return new MarkdownToken(type, value);
        }
    }
}