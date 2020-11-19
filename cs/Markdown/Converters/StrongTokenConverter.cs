namespace Markdown.Converters
{
    public class StrongTokenConverter : TagTokenConverter
    {
        public StrongTokenConverter(IConverter converter) : base(converter, "strong")
        {
        }
    }
}