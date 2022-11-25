namespace Markdown.DataStructures
{
    public class LinkTag : ITag
    {
        public string OpeningTag => $"<a href=\"{Link}\">";
        public string ClosingTag => "</a>";
        public string MarkdownName => $"[{LinkName}]({Link})";
        public string TagContent => LinkName;
        public string Link { get; set; }
        public string LinkName { get; set; }
    }
}