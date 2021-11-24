using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class TokenExtensions
    {
        public static TokenNode ToNode(this Token token) => new(token);

        public static Token ToText(this Token token) => Token.Text(token.Value);
    }
}