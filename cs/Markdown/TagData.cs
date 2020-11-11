namespace Markdown
{
    public class TagData : ITagData
    {
        public string MarkdownTag { get; }

        public string OpenHtmlTag => $"\\<{htmlTag}>";
        public string CloseHtmlTag => $"\\</{htmlTag}>";

        private readonly string htmlTag;

        public TagData(string markdownTag, string htmlTag)
        {
            MarkdownTag = markdownTag;
            this.htmlTag = htmlTag;
        }
    }
}