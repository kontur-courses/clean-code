using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class TokenExtensions
    {
        public static TagNode ToNode(this Token token) => token.ToTag().ToNode();
        
        public static Tag ToTag(this Token token) => token.Type switch
        {
            TokenType.Bold => Tag.Bold(Token.Bold.Value),
            TokenType.Cursive => Tag.Cursive(Token.Cursive.Value),
            _ => Tag.Text(token.Value)
        };
        
        public static Token ToText(this Token token) => Token.Text(token.Value);
    }
}