namespace Markdown.Data
{
    public class TagTranslationInfo
    {
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        public readonly string Translation;

        public TagTranslationInfo(string openingTag, string closingTag, string translation)
        {
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            Translation = translation;
        }
    }
}