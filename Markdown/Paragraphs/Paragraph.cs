namespace Markdown.Paragraphs;

public record Paragraph(
    string Source,
    int Start,
    int End,
    bool IsHeading);

