namespace Markdown.Transducer.ConverterTokenToHtml
{
    public class ConverterDoubleEmphasis : AbstractConverterTokenToHtml

    {
        public ConverterDoubleEmphasis() : base("<strong>", "</strong>")
        {
        }
    }
}