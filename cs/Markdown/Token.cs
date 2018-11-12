namespace Markdown
{
    public struct Token
    {
        public readonly MdType Type;
        public readonly string Value;

        public Token(MdType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}