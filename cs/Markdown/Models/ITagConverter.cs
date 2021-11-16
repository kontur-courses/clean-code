namespace Markdown.Models
{
    public interface ITagConverter
    {
        public string OpenTag { get; init; }
        public string CloseTag { get; init; }
        public int GetTrimFromStartCount { get; init; }
        public int GetTrimFromEndCount { get; init; }
    }
}