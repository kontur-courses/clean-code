using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions;

public interface ILineParser
{
    IElement ParseContentLine(string content);
}