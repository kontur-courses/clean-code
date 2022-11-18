using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface ILineElementParser : IElementParser
{
    IElement ParseElement(string content);
}