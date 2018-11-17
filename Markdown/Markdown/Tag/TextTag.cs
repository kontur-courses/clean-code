namespace Markdown.Tag
{
    public class TextTag : ITag
    {
        public string Symbol { get; set; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string HtmlOpen { get; set; }
        public string HtmlClose { get; set; }
        public int Length { get; set; }
        public string Content { get; set; }
    }
}
