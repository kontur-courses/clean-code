namespace Markdown
{
    public class Token
    {
        public string Line { get; }
        public int Start { get; }
        public int Length { get; }
        public string Tag { get; } = null;
        public Token(string line, int start, int length,string tag)
        {
            Line = line;
            Start = start;
            Length = length;
            Tag = tag;
        }

        public Token(string line, int start, int length)
        {
            Line = line;
            Start = start;
            Length = length;
        }
        public override bool Equals(object obj)
        {
            var token = obj as Token;
            return token.Line == Line && token.Start == Start && token.Length == Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Line.GetHashCode() + Start * 8069 + Length;
            }
        }
    }
}