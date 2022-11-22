namespace Markdown.Tokens
{
    public class Token
    {
        public int Start { get; }

        public int Length { get; }

        public int End => Start + Length - 1;

        public Token(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
