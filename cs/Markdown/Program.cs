using Markdown.Extensions;
using Markdown.Parsers;
using Markdown.Renderers;

namespace Markdown;

internal class Program
{
    private static readonly Md Md = new(new MarkdownParser(), new HtmlRenderer());

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