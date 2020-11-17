using System;

namespace Markdown
{
    public class HeaderTagHelper : TagHelper
    {
        private HeaderTagHelper()
            : base("# ", "<h1>")
        {
        }

        public override bool TryParse(int position, string text, out Tag tag, bool inWord = false)
        {
            if (IsTag(position, text) && IsAfterNewLine(position, text))
            {
                tag = new Tag(position, TagType.Header, true, MdTag.Length, inWord, false);
                return true;
            }

            tag = null;
            return false;
        }

        public override bool ParseForEscapeTag(int position, string text)
        {
            return IsTag(position + 1, text) && IsAfterNewLine(position, text);
        }

        public static TagHelper CreateInstance()
        {
            return new HeaderTagHelper();
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