using System;

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

        public static bool TryPairCloseTag(this Tag tag, Tag possibleCloseTag, string text)
        {
            if (!possibleCloseTag.CanBeClose(text) || IsEmptyStringBetween(tag, possibleCloseTag))
                return false;
            if (tag.InWord || possibleCloseTag.InWord)
                if (tag.NotInOneWordWith(possibleCloseTag, text))
                    return false;
            possibleCloseTag.ConvertToClose();
            return true;
        }

        public static bool NotInOneWordWith(this Tag tag, Tag otherTag, string text)
        {
            return text.AsSpan(tag.Position, otherTag.Position - tag.Position).Any(char.IsWhiteSpace);
        }

        private static bool IsEmptyStringBetween(Tag openTag, Tag closeTag)
        {
            return closeTag.Position - openTag.Position == closeTag.MdTagLength;
        }
    }
}