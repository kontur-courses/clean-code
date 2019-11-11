namespace Markdown
{
    public class TagType
    {
        public readonly string HtmlOpeningTag;
        public readonly string HtmlClosingTag;
        public readonly string OpeningSymbol;
        public readonly string ClosingSymbol;

        public TagType(string htmlOpeningTag, string htmlClosingTag, string openingSymbol, string closingSymbol)
        {
            HtmlOpeningTag = htmlOpeningTag;
            HtmlClosingTag = htmlClosingTag;
            OpeningSymbol = openingSymbol;
            ClosingSymbol = closingSymbol;
        }
    }
}