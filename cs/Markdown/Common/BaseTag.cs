using Markdown.Extensions;

namespace Markdown.Common
{
    public abstract class BaseMdTag
    {
        public string MdTag { get; }
        public string HtmlOpenTag { get; }
        public string HtmlCloseTag { get; }
        public int Length => MdTag.Length;
        public bool IsInLine { get; }
        public bool HasCloseMdTag { get; protected set; }


        protected BaseMdTag()
        {
            MdTag = string.Empty;
            HtmlOpenTag = string.Empty;
            HtmlCloseTag = string.Empty;
            IsInLine = true;
            HasCloseMdTag = true;
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


        public virtual bool IsTag(string text, int pos)
        {
            return text.IsSubstring(pos, MdTag);
        }

        protected abstract bool CanCreateToken(string text, int startIndex, int stopIndex);

        public abstract bool TryGetToken(string text, int startIndex, out Token token);

        public abstract string RemoveMdTags(string value);

        public abstract string InsertHtmlTags(string text);
    }
}