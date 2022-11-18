using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownParagraphParser : ILineElementParser
{
    public Type ParsingElementType { get; } = typeof(ParagraphElement);

    IElement ILineElementParser.ParseElement(string content) =>
        ParseElement(content);

    public ParagraphElement ParseElement(string content) =>
        new(content);
}