namespace Markdown
{
    public abstract class Token
    {
        public int StartPosition;
        public int Length;
        public Token Parent;

        public Token(int startPosition = 0, int length = 0, Token parent = null)
        {
            StartPosition = startPosition;
            Length = length;
            Parent = parent;
        }
    }
}