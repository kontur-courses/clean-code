namespace Markdown
{
    public class SingleToken : IToken
    {
        public AttributeType Type { get; }
        public int Position { get; }
        public int AttributeLength { get; }

        public SingleToken(AttributeType type, int position, int attributeLength = 1)
        {
            Type = type;
            Position = position;
            AttributeLength = attributeLength;
        }
    }
}