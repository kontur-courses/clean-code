namespace Markdown;

public interface IParagraphCreator
{
    public List<Paragraph> GetParagraphs(string markdownString);
}