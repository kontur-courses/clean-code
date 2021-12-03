using System;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class BlockMdTag : BaseMdTag
    {
        public BlockMdTag()
            : base()
        {
            HasCloseMdTag = false;
        }

        public BlockMdTag(string mdTag, string htmlOpenTag, string htmlCloseTag)
            : base(mdTag, htmlOpenTag, htmlCloseTag)
        {
            HasCloseMdTag = false;
        }

        public override bool IsTag(string text, int pos)
        {
            return base.IsTag(text, pos) &&
                   (text.IsSubstring(pos, Environment.NewLine, false) || pos == 0);
        }
        
        protected override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            return IsTag(text, startIndex) &&
                   (text.IsSubstring(stopIndex, Environment.NewLine, false) || text.Length == stopIndex);
        }
        
        public override bool TryGetToken(string text, int startIndex, out Token token)
        {
            var closeTagIndex = text.GetEndOfLine();
            if (CanCreateToken(text, startIndex, closeTagIndex))
            {
                token = text.GetToken(startIndex, closeTagIndex, this);
                return true;
            }

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