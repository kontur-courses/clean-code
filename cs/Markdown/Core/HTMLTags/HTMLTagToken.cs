using Markdown.Core.Tokens;

namespace Markdown.Core.HTMLTags
{
    public class HTMLTagToken : Token, IHTMLTagToken
    {
        public HTMLTagType TagType { get; }

        public HTMLTagToken(int position,  string value, HTMLTagType tagType)
            : base(position, value, Tokens.TokenType.HTMLTag)
        {
            TagType = tagType;
        }
    }
}