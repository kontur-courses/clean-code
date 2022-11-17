namespace MarkdownRenderer.Abstractions;

public interface ISpecialLineElementParser : ISpecialElementParser, ILineElementParser
{
    bool Match(string line);
    bool TryParseElement(string content, out IElement? element);
}