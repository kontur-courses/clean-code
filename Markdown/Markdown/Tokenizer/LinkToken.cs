namespace Markdown
{
    public class LinkToken : IToken
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public int AttributeLength { get; }

        public string URL { get; }

        public LinkToken(int position, int attributeLength, string url)
        {
            Type = AttributeType.Link;
            Position = position;
            AttributeLength = attributeLength;
            URL = url;
        }
    }
}