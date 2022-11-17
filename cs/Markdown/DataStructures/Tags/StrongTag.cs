namespace Markdown.DataStructures
{
    public class StrongTag : ITag
    {
        public string OpeningTag => "<strong>";
        public string ClosingTag => "</strong>";
        public string MarkdownName => "__";
    }
}