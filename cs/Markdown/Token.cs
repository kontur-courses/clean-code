namespace Markdown
{
    public class Token
    {
        public Token(TokenInformation data, TokenType type, int position)
        {
            Data = data;
            TokenType = type;
            Position = position;
        }

        public TokenInformation Data { get; }
        public TokenType TokenType { get; }
        public int Position { get; }
    }
}