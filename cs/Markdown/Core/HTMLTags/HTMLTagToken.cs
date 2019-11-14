using Markdown.Core.Tokens;

namespace Markdown.Core.HTMLTags
{
    public class HTMLTagToken : Token, IHTMLTagToken
    {
        public bool IsOpen { get; set; }

        public HTMLTagToken(int position,  string value, bool isOpen)
            : base(position, value, Tokens.TokenType.HTMLTag)
        {
            IsOpen = isOpen;
        }
    }
}