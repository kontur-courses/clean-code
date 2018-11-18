namespace Markdown.Tag
{
    public class TextTag : ITag
    {
        public string Symbol { get; set; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html { get; set; }
        public int Length { get; set; }
        public string Content { get; set; }
        public MdType Type { get; set; } = MdType.Text;
    }
}
