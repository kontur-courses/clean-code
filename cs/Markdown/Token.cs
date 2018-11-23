namespace Markdown
{
    public class Token
    {
        public readonly string Value;
        public readonly int Length;
        public readonly bool IsControl;

        public Token(string value, bool isControl)
        {
            Value = value;
            Length = value.Length;
            isControl = isControl;
        }

        public override int GetHashCode() =>
            Value.GetHashCode() + (IsControl ? 100 : 56);

        public override bool Equals(object obj)
        {
            if (obj is Token token)
                return Value == token.Value && IsControl == token.IsControl;

            return false;
        }
    }
}