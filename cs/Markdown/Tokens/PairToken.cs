namespace Markdown
{
    public class PairToken : Token
    {
        public bool IsFirst;

        public PairToken(TokenType type, string content, int length, bool isFirst) : base(type, content, length)
        {
            IsFirst = isFirst;
        }

        public PairToken(TokenType type, string content, int length) : base(type, content, length)
        {
        }

        public PairToken(TokenType type, string content) : base(type, content)
        {
        }

        public PairToken(TokenType type) : base(type)
        {
        }
    }
}