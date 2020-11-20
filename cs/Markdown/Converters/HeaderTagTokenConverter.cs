namespace Markdown.Converters
{
    public class HeaderTagTokenConverter : TagTokenConverter
    {
        public HeaderTagTokenConverter(IConverter converter) : base(converter, "h1")
        {
        }
    }
}