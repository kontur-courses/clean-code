namespace Markdown
{
    public interface ITagData
    {
        public string MarkdownTag { get; }
        public string OpenHtmlTag { get; }
        public string CloseHtmlTag { get; }
    }
}