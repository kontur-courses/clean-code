namespace Markdown
{
    public class EncloserToken : Token
    {
        public int Start;
        public int End;

        public EncloserToken(string value, TokenType type, int start, int end) : base(value, type)
        {
            Start = start;
            End = end;
        }
    }
}
