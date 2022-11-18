using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface ISpecialLineElementParser : ISpecialElementParser, ILineElementParser
{
    bool Match(string line);
    bool TryParseElement(string content, out IElement? element);
}