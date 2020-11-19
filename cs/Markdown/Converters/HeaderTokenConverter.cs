namespace Markdown.Converters
{
    public class HeaderTokenConverter : TagTokenConverter
    {
        public HeaderTokenConverter(IConverter converter) : base(converter, "h1")
        {
        }
    }
}