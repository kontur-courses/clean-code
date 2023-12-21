using Markdown.Tokens;

namespace Markdown
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Content { get; }
        
        public Token(TokenType tokenType, string content)
        {
            TokenType = tokenType;
            Content = content;
        }
    }
}
