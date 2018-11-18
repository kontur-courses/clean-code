namespace Markdown.Tag
{
    public class SharpTag : ITag
    {
        public SharpTag()
        {
            Length = Symbol.Length;
        }

        public string Symbol { get; set; } = "#";
        public int Length { get; set; }
        public string Content { get; set; }
        public MdType Type { get; set; } = MdType.Sharp;
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html { get; set; } = "h1";
    }
}