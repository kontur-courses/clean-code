using System;

namespace Markdown
{
    public class HeaderTag : Tag
    {
        private HeaderTag(int position, string mdTag, bool isOpening)
            : base(mdTag, "<h1>", position, isOpening)
        {
        }

        public override bool TryParse(int position, string text, out Tag tag)
        {
            if (IsTag(position, text) && IsAfterNewLine(position, text))
            {
                tag = new HeaderTag(position, "# ", true);
                return true;
            }

            tag = null;
            return false;
        }

        public override bool ParseForEscapeTag(int position, string text)
        {
            return IsTag(position + 1, text) && IsAfterNewLine(position, text);
        }

        public static HeaderTag GetCloseTag(int position)
        {
            return new HeaderTag(position, "", false);
        }


        public override int GetMdTagLengthToSkip()
        {
            return IsOpening ? MdTag.Length : 0;
        }

        public static Tag CreateInstance()
        {
            return new HeaderTag(0, "# ", true);
        }

        private bool IsAfterNewLine(int position, string text)
        {
            return position == 0
                   || position - Environment.NewLine.Length >= 0
                   && text.Substring(position - Environment.NewLine.Length, Environment.NewLine.Length) ==
                   Environment.NewLine;
        }
    }
}