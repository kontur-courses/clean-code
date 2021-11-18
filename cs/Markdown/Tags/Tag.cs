namespace Markdown.Tags
{
    public abstract class Tag
    {
        public abstract string MdTag { get; }
        public abstract string HtmlTag { get; }
        
        public string GetOpeningHtmlTag() => $"<{HtmlTag}>";
        public string GetClosingHtmlTag() => $"</{HtmlTag}>";
    }
}