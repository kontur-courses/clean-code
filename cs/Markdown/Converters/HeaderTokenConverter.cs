namespace Markdown.Converters
{
    public class HeaderTokenConverter : TokenConverter
    {
        public HeaderTokenConverter(IConverter converter) : base(converter, "h1")
        {
        }
    }
}