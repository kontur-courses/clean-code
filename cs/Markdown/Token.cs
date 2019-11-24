namespace Markdown
{
    public class Token
    {
        public string Line { get; }
        public int Start { get; }
        public int Length { get; }
        public string Tag { get; } = null;
        public int OpenTagLength { get; } = 0;
        public Token(string line, int start, int length,string tag)
        {
            Line = line;
            Start = start;
            Length = length;
            Tag = tag;
        }

        public Token(string line, int start, int length, string tag, int openTagLength)
        {
            Line = line;
            Start = start;
            Length = length;
            Tag = tag;
            OpenTagLength = openTagLength;
        }

        public Token(string line, int start, int length)
        {
            Line = line;
            Start = start;
            Length = length;
        }

        public Token(string line, int start, int length, int openTagLength)
        {
            Line = line;
            Start = start;
            Length = length;
            OpenTagLength = openTagLength;
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