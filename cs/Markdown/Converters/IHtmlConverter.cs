namespace Markdown.Converters;

public interface IHtmlConverter
{
    string Convert(List<MarkdownParagraph> markdownParagraphs);
}