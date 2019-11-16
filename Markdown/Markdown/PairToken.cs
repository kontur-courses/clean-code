namespace Markdown
{
    public class PairToken : Token
    {
        public readonly bool IsClosing;

        public PairToken(AttributeType type, int position, bool isClosing, int attributeLength = 1) 
            : base(type, position, attributeLength)
        {
            IsClosing = isClosing;
        }
    }
}