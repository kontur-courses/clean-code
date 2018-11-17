namespace Markdown.Tag
{
    public class DoubleUnderLineTag : ITag
    {
        public string Symbol { get; set; } = "__";
        public int Length { get; set; } = 2;
        public string Content { get; set; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string HtmlOpen { get; set; } = "<strong>";
        public string HtmlClose { get; set; } = "</strong>";
    }
}