using Markdown.Extensions;

namespace Markdown;

internal class Program
{
    private static void Main(string[] args)
    {
        var markdownText = """
                           _курсив_
                           # заголовок
                           __полужирный__
                           """;

        var htmlText = Md.Render(markdownText);

        Console.WriteLine(htmlText);

        Console.WriteLine();

        var htmlTextByExtensions = markdownText
            .ParseMarkdown()
            .RenderHtml();

        Console.WriteLine(htmlTextByExtensions);
    }
}