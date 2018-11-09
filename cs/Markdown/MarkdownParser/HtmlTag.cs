namespace Markdown
{
    public class HtmlTag
    {
        public readonly string Content;
        public readonly ISpanElement SpanElement;

        public HtmlTag(string content, ISpanElement spanElement = null)
        {
            Content = content;
            SpanElement = spanElement;
        }

        public bool HasHtmlWrap()
        {
            return SpanElement != null;
        }

        public string ToHtml()
        {
            return SpanElement?.ToHtml(Content) ?? Content;
        }
    }
}