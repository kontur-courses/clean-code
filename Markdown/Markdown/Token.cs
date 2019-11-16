namespace Markdown
{
    public class Token
    {
        public readonly AttributeType Type;
        public readonly int Position;
        public readonly int AttributeLength;

        public Token(AttributeType type, int position, int attributeLength = 1)
        {
            Type = type;
            Position = position;
            AttributeLength = attributeLength;
        }
    }
}