namespace Markdown
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

        public override bool Equals(object obj)
        {
            if ((Token) obj == null)
                return false;
            return Equals((Token) obj);
        }

        protected bool Equals(Token other)
        {
            return End == other.End && Start == other.Start && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = End;
                hashCode = (hashCode * 397) ^ Start;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var value = $"[{Value}]";
            return $"{value.PadRight(10)} Position={Start:##0} Length={End}";
        }
    }
}