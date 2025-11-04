namespace Markdown;

public class Paragraph
{
    public string Text { get; private set; }
    public ParagraphType Type { get; private set; }

    public Paragraph(string text, ParagraphType type)
    {
        Text = text;
        Type = type;
    }
}