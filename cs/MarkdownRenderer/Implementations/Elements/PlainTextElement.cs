using MarkdownRenderer.Abstractions.Elements;

namespace MarkdownRenderer.Implementations.Elements;

public class PlainTextElement : StandardElement
{
    public string Content { get; }

    public PlainTextElement(string content)
    {
        Content = content;
    }
}