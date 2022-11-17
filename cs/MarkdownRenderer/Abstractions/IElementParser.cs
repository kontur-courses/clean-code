namespace MarkdownRenderer.Abstractions;

public interface IElementParser
{
    Type ParsingElementType { get; }
    ElementParseType ParseType { get; }
    bool IsElementStart(string content, int index);
    bool IsElementEnd(string content, int index);
    bool TryParseElement(string source, Token token, out IElement? element);
}