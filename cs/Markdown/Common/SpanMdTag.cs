using System;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class SpanMdTag : BaseMdTag
    {
        public SpanMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag)
            : base(mdTag, htmlOpenTag, htmlCloseTag)
        {
        }

        public override bool IsTag(string text, int pos)
        {
            return base.IsTag(text, pos) &&
                   text.IsSubstring(pos, char.IsDigit, false) != true &&
                   text.IsSubstring(pos + MdTag.Length, char.IsDigit) != true;
        }

        protected override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            if (!IsTag(text, startIndex) || !IsTag(text, stopIndex))
                return false;

            var value = text.Substring(startIndex, stopIndex + MdTag.Length - startIndex);
            if (value.Split(' ').Length == 1)
                return value.Length > MdTag.Length * 2;

            return value.Split(Environment.NewLine).Length == 1 &&
                   text.IsSubstring(startIndex, char.IsWhiteSpace, false) != false &&
                   text.IsSubstring(stopIndex + MdTag.Length, char.IsWhiteSpace) != false;
        }

        public override bool TryGetToken(string text, int startIndex, out Token token)
        {
            var stopSearchIndex = IsInLine ? text.GetEndOfLine() : text.Length;
            var closeTagIndex = text.IndexOf(MdTag, startIndex + MdTag.Length, StringComparison.Ordinal);
            while (closeTagIndex < stopSearchIndex && closeTagIndex != -1)
            {
                if (CanCreateToken(text, startIndex, closeTagIndex))
                {
                    token = text.GetToken(startIndex, closeTagIndex + MdTag.Length, this);
                    return true;
                }

                closeTagIndex = text.IndexOf(MdTag, closeTagIndex + MdTag.Length, StringComparison.Ordinal);
            }

            token = null;
            return false;
        }

        public override string RemoveMdTags(string value)
        {
            return value.Remove(value.Length - MdTag.Length).Remove(0, MdTag.Length);
        }

        public override string InsertHtmlTags(string text)
        {
            return text.Insert(text.Length, HtmlCloseTag).Insert(0, HtmlOpenTag);
        }
    }
}