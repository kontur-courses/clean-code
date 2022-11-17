namespace MarkdownRenderer.Abstractions;

public interface ILineElementParser : IElementParser
{
    IElement ParseElement(string content);
}