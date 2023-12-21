namespace Markdown.Converters
{
    public interface IConverter
    {
        public string InsertTags(ParsedText paragraphInfo);
    }
}
