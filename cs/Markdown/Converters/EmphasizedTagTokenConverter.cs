namespace Markdown.Converters
{
    public class EmphasizedTagTokenConverter : TagTokenConverter
    {
        public EmphasizedTagTokenConverter(IConverter converter) : base(converter, "em")
        {
        }
    }
}