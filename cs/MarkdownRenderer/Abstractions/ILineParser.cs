using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Abstractions;

public interface ILineParser
{
    IElement ParseLineContent(string content);
}