using Markdown.Tags;

namespace Markdown.Converter
{
    public class MarkdownToHtmlConverter : Converter
    {
        public MarkdownToHtmlConverter() : base(new MdTagStorage(), new HtmlTagStorage())
        {
        }
    }
}
