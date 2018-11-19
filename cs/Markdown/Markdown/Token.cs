namespace Markdown
{
    public struct Token
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public int ContentLeftOffset { get; set; }
        public int ContentLength { get; set; }
    }
}