using Markdown.Extensions;
using Markdown.Rendering;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var htmlBuilder = new HtmlBuilder();

        foreach (var paragraph in text.GetParagraphs())
            htmlBuilder.Append(paragraph.Tokenize().Resolve());

        return htmlBuilder.ToString();
    }
}