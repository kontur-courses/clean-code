namespace Markdown.Converters
{
    public class HeadingTokenConverter : TagConvertor
    {
        public HeadingTokenConverter(IConverter converter) : base(converter, "h1")
        {
        }
    }
}