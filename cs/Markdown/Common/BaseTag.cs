using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Common
{
    public abstract class BaseMdTag
    {
        public string MdTag { get; }
        protected string HtmlOpenTag { get; }
        protected string HtmlCloseTag { get; }
        public int Length => MdTag.Length;
        public bool IsMultiLine { get; protected set; }

        protected BaseMdTag()
        {
            MdTag = string.Empty;
            HtmlOpenTag = string.Empty;
            HtmlCloseTag = string.Empty;
            IsMultiLine = false;
        }

        protected BaseMdTag(string mdTag)
            : this()
        {
            MdTag = mdTag;
        }

        protected BaseMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag)
            : this(mdTag)
        {
            HtmlOpenTag = htmlOpenTag;
            HtmlCloseTag = htmlCloseTag;
        }

        protected virtual bool IsTag(string text, int pos)
        {
            return text.IsSubstring(pos, MdTag);
        }

        public abstract bool CanCreateToken(string text, int startIndex, int stopIndex);

        public abstract bool TryGetToken(string text, Tag openTag, IList<Tag> closeTags, out Token token,
            out Tag closeTag);

        public abstract string RemoveMdTags(string value);

        public abstract string InsertHtmlTags(string text);
    }
}