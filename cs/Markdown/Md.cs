using Markdown.Abstractions;

namespace Markdown;

public class Md(ITextParser parser, IMarkedTextRenderer renderer)
{
    public string Render(string markdownText)
    {
        var tokens = parser.Parse(markdownText);

        return renderer.Render(tokens);
    }
}