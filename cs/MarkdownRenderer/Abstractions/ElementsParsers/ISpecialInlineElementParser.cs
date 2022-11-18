using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface ISpecialInlineElementParser : ISpecialElementParser, IInlineElementParser
{
    bool IsElementStart(string content, int index);
    bool IsElementEnd(string content, int index);
    bool TryParseElement(string content, Token token, out IElement? element);
}