namespace Markdown.Transducer.ConverterTokenToHtml
{
    public class ConverterSingleEmphasis : AbstractConverterTokenToHtml
    {
        public ConverterSingleEmphasis() : base("<em>", "</em>")
        {
        }
    }
}