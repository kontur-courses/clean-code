using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public readonly string OpeningTag;
        public readonly bool CanBeInsideOtherTag;
        public readonly string ClosingTag;
        public readonly string Translation;

        public Tag(string openingTag, bool canBeInsideOtherTag, string closingTag, string translation)
        {
            OpeningTag = openingTag;
            CanBeInsideOtherTag = canBeInsideOtherTag;
            ClosingTag = closingTag;
            Translation = translation;
        }

        public TagInfo ToInfo() => new TagInfo(OpeningTag, CanBeInsideOtherTag, ClosingTag);
    }
}