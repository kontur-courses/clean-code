namespace Markdown.DTOs
{
    public class Token
    {
        public readonly int End;
        public readonly int Start;
        public readonly string Value;

        public Token(string value, int start, int end)
        {
            Start = start;
            End = end;
            Value = value;
        }
    }
}