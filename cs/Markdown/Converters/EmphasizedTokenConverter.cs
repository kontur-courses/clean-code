namespace Markdown.Converters
{
    public class EmphasizedTokenConverter : TokenConverter
    {
        public EmphasizedTokenConverter(IConverter converter) : base(converter, "em")
        {
        }
    }
}