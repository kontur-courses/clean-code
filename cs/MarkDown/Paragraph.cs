namespace MarkDown;

public class Paragraph(ParagraphType type, string content)
{
    public ParagraphType Type { get; set; } = type;
    public string Content  { get; set; } = content;
}