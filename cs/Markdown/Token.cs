namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public TokenType Type { get; }
        public string Content { get; }

        public Token(TokenType type, string content, int length)
        {
            Length = length;
            Type = type;
            Content = content;
        }
    }
}
