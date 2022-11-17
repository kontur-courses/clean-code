using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public abstract class MarkdownElementParser<TElem> : IElementParser
    where TElem : IElement
{
    public Type ParsingElementType => typeof(TElem);
    public abstract ElementParseType ParseType { get; }
    public abstract bool IsElementStart(string content, int index);

    public abstract bool IsElementEnd(string content, int index);

    bool IElementParser.TryParseElement(string content, Token contentToken, out IElement? element)
    {
        var result = TryParseElement(content, contentToken, out var tElement);
        element = tElement;
        return result;
    }

    public abstract bool TryParseElement(string content, Token contentToken, out TElem? italicElement);
}