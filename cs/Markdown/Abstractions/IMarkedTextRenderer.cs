using Markdown.Models;

namespace Markdown.Abstractions;

public interface IMarkedTextRenderer
{
    public string Render(MarkedText markedText);
}