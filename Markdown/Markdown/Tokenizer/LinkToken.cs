using System.Collections.Generic;

namespace Markdown
{
    public class LinkToken : IToken
    {
        public AttributeType Type { get; }

        public int Position { get; }

        public int AttributeLength { get; }

        public  List<IToken> UrlTokens { get; }

        public string RawUrl { get; }

        public LinkToken(int position, int attributeLength, List<IToken> urlTokens, string rawUrl)
        {
            Type = AttributeType.Link;
            Position = position;
            AttributeLength = attributeLength;
            UrlTokens = urlTokens;
            RawUrl = rawUrl;
        }
    }
}