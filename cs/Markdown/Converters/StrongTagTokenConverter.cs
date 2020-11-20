namespace Markdown.Converters
{
    public class StrongTagTokenConverter : TagTokenConverter
    {
        public StrongTagTokenConverter(IConverter converter) : base(converter, "strong")
        {
        }
    }
}