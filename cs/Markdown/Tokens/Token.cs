namespace Markdown.Tokens
{
    public class Token : IToken
    {
        public Token(string content, TokenType tokenType, int startPosition)
        {
            Content = content;
            Type = tokenType;
            StartPosition = startPosition;
        }

        public string Content { get; set; }
        public TokenType Type { get; set; }
        public int StartPosition { get; set; }
    }
}
