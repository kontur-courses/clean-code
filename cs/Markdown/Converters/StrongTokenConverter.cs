namespace Markdown.Converters
{
    public class StrongTokenConverter : TagConvertor
    {
        public StrongTokenConverter(IConverter converter) : base(converter, "strong")
        {
        }
    }
}