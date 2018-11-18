namespace Markdown.Tag
{
    public class DoubleUnderLineTag : ITag
    {
        public DoubleUnderLineTag()
        {
            Length = Symbol.Length;
        }

        public string Symbol { get; set; } = "__";
        public int Length { get; set; }
        public string Content { get; set; }
        public MdType Type { get; set; } = MdType.DoubleUnderLine;
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html { get; set; } = "strong";
    }
}