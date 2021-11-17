namespace Markdown
{
    public class Token
    {
        public readonly int Start;
        public readonly int Length;
        public readonly string Value;

        public Token(int start, int length, string value)
        {
            Start = start;
            Length = length;
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Token token)
            {
                return Start == token.Start
                       && Length == token.Length
                       && Value == token.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() * 18253 + Length.GetHashCode() * 3571 + Value.GetHashCode() * 12289;
            }
        }
    }
}