using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokens
{
    public abstract class MarkdownToken : Token
    {
        protected MarkdownToken(string value, string selector, int paragraphIndex, int startIndex) : base(value, selector, paragraphIndex, startIndex)
        {
        }

        public virtual string OpenHtmlTag => string.Empty;
        public virtual string CloseHtmlTag => string.Empty;

        public IEnumerable<MarkdownToken> SubTokens { get; set; }

        public virtual string GetHtmlFormatted()
        {
            var innerValue = SubTokens
                .Select(t => t.GetHtmlFormatted());
            return $"{OpenHtmlTag}{string.Concat(innerValue)}{CloseHtmlTag}";
        }
    }
}