namespace Markdown.Models
{
    public interface IRenderStyle
    {
        public string GetStart { get; set; }
        public string GetEnd { get; set; }
        public int GetTrimFromStartCount { get; set; }
        public int GetTrimFromEndCount { get; set; }
    }
}