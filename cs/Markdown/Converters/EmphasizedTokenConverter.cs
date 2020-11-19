namespace Markdown.Converters
{
    public class EmphasizedTokenConverter : TagTokenConverter
    {
        public EmphasizedTokenConverter(IConverter converter) : base(converter, "em")
        {
        }
    }
}