using System;

namespace Markdown
{
    public abstract class LineTagHelper : TagHelper
    {
        private readonly TagType tagType;

        protected LineTagHelper(string mdTag, string htmlTag, TagType tagType)
            : base(mdTag, htmlTag)
        {
            this.tagType = tagType;
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            if (IsTag(position, text) && IsAfterNewLine(position, text))
            {
                tag = new Tag(position, tagType, true, MdTag.Length, inWord, false);
                return true;
            }

            tag = null;
            return false;
        }

        public override bool ParseForEscapeTag(int position, string text)
        {
            return IsTag(position + 1, text) && IsAfterNewLine(position, text);
        }

        private static bool IsAfterNewLine(int position, string text)
        {
            return position == 0
                   || position - Environment.NewLine.Length >= 0
                   && text.Substring(position - Environment.NewLine.Length, Environment.NewLine.Length) ==
                   Environment.NewLine;
        }
    }
}