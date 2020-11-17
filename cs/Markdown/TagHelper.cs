using System;

namespace Markdown
{
    public abstract class TagHelper
    {
        private readonly string htmlTag;

        protected TagHelper(string mdTag, string htmlTag)
        {
            MdTag = mdTag;
            this.htmlTag = htmlTag;
        }

        protected string MdTag { get; }


        public abstract bool TryParse(int position, string text, out Tag tag, bool inWord = false, int lineNumber = 0);

        public virtual bool ParseForEscapeTag(int position, string text)
        {
            return IsTag(position + 1, text);
        }

        public string GetHtmlTag(bool isOpening)
        {
            return isOpening ? htmlTag : htmlTag.Insert(1, "/");
        }

        public virtual int GetSymbolsCountToSkipForParsing()
        {
            return MdTag.Length;
        }

        protected bool IsTag(int position, string text)
        {
            return position + MdTag.Length <= text.Length
                   && text.Substring(position, MdTag.Length) == MdTag;
        }
    }
}