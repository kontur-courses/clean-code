using System;

namespace Markdown.Tokens
{
    public abstract class MarkdownToken : Token
    {
        protected MarkdownToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }

        public virtual string OpenHtmlTag => string.Empty;
        public virtual string CloseHtmlTag => string.Empty;

        public abstract string GetHtmlFormatted();
    }
}