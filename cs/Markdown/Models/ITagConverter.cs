namespace Markdown.Models
{
    public interface ITagConverter
    {
        public string HtmlOpenTag { get; }
        public string HtmlCloseTag { get; }
        public int TrimFromStartCount { get; }
        public int TrimFromEndCount { get; }
    }
}