using Markdown.Models;

namespace Markdown.Tokens
{
    public class ItalicRenderStyle : IRenderStyle
    {
        public string GetStart { get; set; }
        public string GetEnd { get; set; }
        public int GetTrimFromStartCount { get; set; }
        public int GetTrimFromEndCount { get; set; }
    }
}