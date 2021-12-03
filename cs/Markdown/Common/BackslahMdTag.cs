using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Common
{
    public class BackslashMdTag : BaseMdTag
    {
        public BackslashMdTag()
            : base("\\")
        {
        }

        public override bool CanCreateToken(string text, int startIndex, int stopIndex)
        {
            return IsTag(text, startIndex);
        }

        public override bool TryGetToken(string text, Tag openTag, IList<Tag> closeTags, out Token token,
            out Tag closeTag)
        {
            var openTagIndex = closeTags.IndexOf(openTag);
            var backslashedTag = openTagIndex + 1 > closeTags.Count 
                ? null 
                : closeTags[openTagIndex + 1];
            
            if (backslashedTag != null)
            {
                closeTag = backslashedTag;
                token = text.GetToken(openTag.Position, backslashedTag.Position + backslashedTag.MdTagType.Length, this);
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
            return text;
        }
    }
}