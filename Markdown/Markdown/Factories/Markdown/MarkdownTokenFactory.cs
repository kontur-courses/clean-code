using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Factories.Markdown
{
    public class MarkdownTokenFactory : ITokenFactory<MarkdownToken>
    {
        public MarkdownToken NewToken(TokenType type, string value, IEnumerable<MarkdownToken> childTokens)
        {
            return new MarkdownToken(type, value, childTokens);
        }
    }
}