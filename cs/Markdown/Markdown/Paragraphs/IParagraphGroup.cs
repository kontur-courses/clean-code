namespace Markdown.Paragraphs
{
    public interface IParagraphGroup
    {
        public ParagraphType Type { get; }
        public string OpenTag { get; }
        public string CloseTag { get; }
    }
}