using Markdown.Primitives.TokenHelper;

namespace Markdown.Primitives
{
    public class Token
    {
        public TokenTypes Type { get; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }

        public Token(TokenTypes type, int startPosition, int endPosition)
        {
            Type = type;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
