namespace Markdown.Tag
{
    public class SingleUnderLineTag : ITag
    {
        public SingleUnderLineTag()
        {
            Length = Symbol.Length;
        }

        public string Symbol { get; set; } = "_";
        public int Length { get; set; }
        public string Content { get; set; }
        public MdType Type { get; set; } = MdType.SingleUnderLine;
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html { get; set; } = "em";
    }
}