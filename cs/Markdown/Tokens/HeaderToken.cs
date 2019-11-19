namespace Markdown
{
    public class HeaderToken : Token
    {
        public HeaderToken(TokenType type, string content, int length) : base(type, content, length)
        {
        }

        public HeaderToken(TokenType type, string content) : base(type, content)
        {
        }

        public HeaderToken(TokenType type) : base(type)
        {
        }
    }
}