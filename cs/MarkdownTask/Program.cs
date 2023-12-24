using MarkdownTask;
using System;
using MarkdownTask.MarkdownParsers;

public static class Program
{
    public static void Main()
    {
        Markdown parser = new Markdown(new ITagParser[]{
            new HeaderTagParser(),
            new ItalicTagParser(),
            new StrongTagParser(),
            new EscapingParsing()
        });
        string markdownText = @"\_Вот это\_, не должно выделиться тегом \<em>";
        //string markdownText = "# Header __with _different_ tags__";
        string htmlText = parser.Render(markdownText);
        Console.WriteLine(htmlText);
    }
}