using Markdown.Models;

namespace Markdown.Abstractions;

public interface ITextParser
{
    public MarkedText Parse(string text);
}