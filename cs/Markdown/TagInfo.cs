namespace Markdown
{
    public class TagInfo
    {
        public readonly string OpeningTag;
        public readonly bool CanBeInsideOtherTag;
        public readonly string ClosingTag;

        public TagInfo(string openingTag, bool canBeInsideOtherTag, string closingTag)
        {
            OpeningTag = openingTag;
            CanBeInsideOtherTag = canBeInsideOtherTag;
            ClosingTag = closingTag;
        }
    }
}