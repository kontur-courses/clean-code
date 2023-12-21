namespace Markdown.Converters
{
    public interface Converter
    {
        public static extern string InsertTags(ParsedText paragraphInfo);
    }
}
