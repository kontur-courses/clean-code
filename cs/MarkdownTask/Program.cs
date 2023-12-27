using MarkdownTask;
using MarkdownTask.MarkdownParsers;

public static class Program
{
    public static void Main()
    {
        var s = "# Header __with _different_ tags__";

        Markdown parser = new Markdown(new ITagParser[]{
            new HeaderTagParser(),
            new ItalicTagParser(),
            new StrongTagParser(),
            new EscapingParsing(),
            new LinkTagParser()
            });

        Console.WriteLine(parser.Render(s));
    }
}