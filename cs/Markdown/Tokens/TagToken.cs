namespace Markdown.Tokens
{
    public class TagToken
    {
        public TagToken(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public int Start { get; set; }
        public int Length { get; set; }
        public int End => Start + Length - 1;
        
    }
}