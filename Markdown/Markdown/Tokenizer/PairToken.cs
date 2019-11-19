namespace Markdown
{
    public class PairToken : IToken 
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public int AttributeLength { get; }

        public readonly bool IsClosing;

        public PairToken(AttributeType type, int position, bool isClosing, int attributeLength = 1)
        {
            Type = type;
            Position = position;
            AttributeLength = attributeLength;
            IsClosing = isClosing;
        }
    }
}