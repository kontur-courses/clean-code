using System;
using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class BlockMdTag : BaseMdTag
    {
        public BlockMdTag()
            : base()
        {
        }

        public BlockMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag)
            : base(mdTag, htmlOpenTag, htmlCloseTag)
        {
        }

        protected override bool IsTag(string text, int pos)
        {
            return base.IsTag(text, pos) &&
                   (text.IsSubstring(pos, Environment.NewLine, false) || pos == 0);
        }

        public override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            return IsTag(text, startIndex) &&
                   (text.IsSubstring(stopIndex, Environment.NewLine, false) || text.Length == stopIndex);
        }

        public override bool TryGetToken(string text, Tag openTag, IList<Tag> closeTags, out Token token,
            out Tag closeTag)
        {
            var closeTagIndex = text.GetEndOfLine(openTag.Position);
            if (CanCreateToken(text, openTag.Position, closeTagIndex))
            {
                closeTag = null;
                token = text.GetToken(openTag.Position, closeTagIndex, this);
                return true;
            }

            closeTag = null;
            token = null;
            return false;
        }

        public override string RemoveMdTags(string value)
        {
            return value.Remove(0, MdTag.Length);
        }

        public override string InsertHtmlTags(string text)
        {
            return text.Insert(text.EndsWith(Environment.NewLine)
                        ? text.Length - Environment.NewLine.Length
                        : text.Length,
                    HtmlCloseTag)
                .Insert(0, HtmlOpenTag);
        }
    }
}