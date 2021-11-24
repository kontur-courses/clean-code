namespace Markdown.Paragraphs
{
    public class Paragraph
    {
        public ParagraphType Type { get; }
        public string Content { get; }

        public Paragraph(ParagraphType type, string content)
        {
            Type = type;
            Content = content;
        }
    }
}