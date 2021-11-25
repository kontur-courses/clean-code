namespace Markdown.Tokens
{
    public class TagToken
    {
        public TagToken(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public int Start { get; }
        public int Length { get; }
    }
}