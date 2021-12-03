using System;
using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class SpanMdTag : BaseMdTag
    {
        public SpanMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag)
            : base(mdTag, htmlOpenTag, htmlCloseTag)
        {
        }

        protected override bool IsTag(string text, int pos)
        {
            return base.IsTag(text, pos) &&
                   text.IsSubstring(pos, char.IsDigit, false) != true &&
                   text.IsSubstring(pos + MdTag.Length, char.IsDigit) != true;
        }

        public override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            if (!IsTag(text, startIndex) || !IsTag(text, stopIndex - MdTag.Length))
                return false;

            var value = text.Substring(startIndex, stopIndex - startIndex);
            if (value.Split(' ').Length == 1)
                return value.Length > MdTag.Length * 2;

            return value.Split(Environment.NewLine).Length == 1 &&
                   text.IsSubstring(startIndex, char.IsWhiteSpace, false) != false &&
                   text.IsSubstring(stopIndex, char.IsWhiteSpace) != false;
        }

        public override bool TryGetToken(string text, Tag openTag, IList<Tag> closeTags, out Token token, out Tag clTag)
        {
            for (var i = 0; i < closeTags.Count; i++)
            {
                if (openTag == closeTags[i] ||
                    openTag.MdTagType != closeTags[i].MdTagType ||
                    openTag.Position > closeTags[i].Position ||
                    !openTag.MdTagType.CanCreateToken(text, openTag.Position, closeTags[i].Position + closeTags[i].MdTagType.Length))
                    continue;

                clTag = closeTags[i];
                token = text.GetToken(openTag.Position, closeTags[i].Position + closeTags[i].MdTagType.Length, openTag.MdTagType);
                return true;
            }

            clTag = null;
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