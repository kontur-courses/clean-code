using System;
using System.Linq;

namespace Markdown
{
    public static class TagExtensions
    {
        public static bool CanBeOpen(this Tag tag, string text)
        {
            var positionAfterTag = tag.Position + tag.MdTagLength;
            return positionAfterTag < text.Length && !char.IsWhiteSpace(text[positionAfterTag]);
        }

        private static bool CanBeClose(this Tag tag, string text)
        {
            var positionBeforeTag = tag.Position - 1;
            return positionBeforeTag > 0 && !char.IsWhiteSpace(text[positionBeforeTag]);
        }

        public static bool TryPairCloseTag(this Tag tag, Tag closeTag, string text)
        {
            if (!closeTag.CanBeClose(text) || IsEmptyStringBetween(tag, closeTag))
                return false;
            if (tag.InWord || closeTag.InWord)
                if (tag.NotInOneWordWith(closeTag, text))
                    return false;
            closeTag.ConvertToClose();
            return true;
        }

        public static bool NotInOneWordWith(this Tag tag, Tag otherTag, string text)
        {
            return text.Substring(tag.Position, otherTag.Position - tag.Position).Any(char.IsWhiteSpace);
        }

        private static bool IsEmptyStringBetween(Tag openTag, Tag closeTag)
        {
            if (openTag.Type != closeTag.Type)
                throw new ArgumentException("Tag types must be equals");
            return closeTag.Position - openTag.Position == closeTag.MdTagLength;
        }
    }
}