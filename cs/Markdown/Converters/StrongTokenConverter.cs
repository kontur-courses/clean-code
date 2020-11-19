namespace Markdown.Converters
{
    public class StrongTokenConverter : TokenConverter
    {
        public StrongTokenConverter(IConverter converter) : base(converter, "strong")
        {
        }
    }
}