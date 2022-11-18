using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface IInlineElementParser : IElementParser
{
    IElement ParseElement(string content, Token token);
}