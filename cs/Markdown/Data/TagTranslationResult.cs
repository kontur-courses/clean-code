namespace Markdown.Data
{
    public class TagTranslationResult
    {
        public readonly string OpeningTranslation;
        public readonly string ClosingTranslation;

        public TagTranslationResult(string openingTranslation, string closingTranslation)
        {
            OpeningTranslation = openingTranslation;
            ClosingTranslation = closingTranslation;
        }
    }
}