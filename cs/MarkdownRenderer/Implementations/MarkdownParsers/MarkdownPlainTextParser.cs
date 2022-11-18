using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;
using MarkdownRenderer.Implementations.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownPlainTextParser : IInlineElementParser
{
    public Type ParsingElementType { get; } = typeof(PlainText);

    IElement IInlineElementParser.ParseElement(string content, Token contentToken) =>
        ParseElement(content, contentToken);

    public PlainText ParseElement(string content, Token contentToken) =>
        new(content.Substring(contentToken));
}