namespace Markdown.Paragraphs
{
    public class UnorderedListParagraphGroup : IParagraphGroup
    {
        public ParagraphType Type { get; } = ParagraphType.UnorderedList;
        public string OpenTag { get; } = "<ul>";
        public string CloseTag { get; } = "</ul>";
    }
}