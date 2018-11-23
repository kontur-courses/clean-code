namespace Markdown
{
    public struct Tag
    {
        public readonly string Md;
        public readonly string Html;
        public readonly string HtmlOpen;
        public readonly string HtmlClose;
        public readonly bool WithClosure;

        public Tag(string md, string html, bool withClosure = true)
        {
            Md = md;
            Html = html;
            HtmlOpen = $"<{html}>";
            WithClosure = withClosure;
            HtmlClose = withClosure ? $"</{html}>" : "";
        }
    }
}
