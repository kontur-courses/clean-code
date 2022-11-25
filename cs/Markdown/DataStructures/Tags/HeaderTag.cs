namespace Markdown.DataStructures
{
    public class HeaderTag : ITag
    {
        public string OpeningTag => "<h1>";
        public string ClosingTag => "</h1>";
        public string MarkdownName => "#";

        public string TagContent => "";
    }
}