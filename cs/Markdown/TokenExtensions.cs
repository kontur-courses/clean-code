
namespace Markdown
{
    public static class TokenExtensions
    {
        public static TokenNode ToNode(this Token token) => new(token);

        public static Token ToText(this Token token) => new(TokenType.Text, token.Value);
    }
}