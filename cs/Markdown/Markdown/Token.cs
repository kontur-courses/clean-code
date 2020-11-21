namespace Markdown
{
    public class Token
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public int End => Start + Length - 1;

        public bool IsMarkup { get; set; }

        public Token(int start, int length, bool isMarkup = false)
        {
            Start = start;
            Length = length;
            IsMarkup = isMarkup;
        }
    }
}