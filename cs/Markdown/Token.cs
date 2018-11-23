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
    }
}