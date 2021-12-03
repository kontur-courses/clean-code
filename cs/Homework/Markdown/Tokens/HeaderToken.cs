using System.Linq;

namespace Markdown.Tokens
{
    public class HeaderToken: MarkdownToken
    {
        public HeaderToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }
        
        public override string OpenHtmlTag => $"<h{Selector.Length}>";
        public override string CloseHtmlTag => $"</h{Selector.Length}>";
    }
}