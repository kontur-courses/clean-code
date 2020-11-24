namespace Markdown.Converters
{
    public class EmphasizedTokenConverter : TagConvertor
    {
        public EmphasizedTokenConverter(IConverter converter) : base(converter, "em")
        {
        }
    }
}