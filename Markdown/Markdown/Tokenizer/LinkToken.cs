using System.Collections.Generic;
using System.Dynamic;

namespace Markdown
{
    public class LinkToken : IToken
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public int AttributeLength { get; }

        public string RawUrl { get; }

        public LinkToken(int position, int attributeLength, string rawUrl)
        {
            Type = AttributeType.Link;
            Position = position;
            AttributeLength = attributeLength;
            RawUrl = rawUrl;
        }
    }
}