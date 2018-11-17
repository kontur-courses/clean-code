namespace Markdown.Tag
{
    public class SharpTag : ITag
    {
        public string Symbol { get; set; } = "#";
        public int Length { get; set; } = 1;
        public string Content { get; set; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string HtmlOpen { get; set; } = "<h1>";
        public string HtmlClose { get; set; } = "</h1>";
    }
}