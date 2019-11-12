namespace Markdown
{
    public class MarkTranslatorElement
    {
        public readonly string Mark;
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        
        public MarkTranslatorElement(string mark, string openingTag, string closingTag)
        {
            Mark = mark;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
        }
    }
}