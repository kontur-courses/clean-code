using Markdown.Extensions;
using Markdown.Rendering;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var htmlBuilder = new HtmlBuilder();

        var tokens = text.GetParagraphs()
            .SelectMany(paragraph => paragraph.Tokenize().Resolve());

        htmlBuilder.Append(tokens);

        return htmlBuilder.ToString();
    }
}