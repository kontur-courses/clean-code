using MarkdownTask;
using MarkdownTask.MarkdownParsers;

public static class Program
{
    public static void Main()
    {
        var s = "# Header __with _different_ tags__";

        Markdown parser = new(new ITagParser[]{
            new HeaderTagParser(),
            new PairedTagsParser("_", TagInfo.TagType.Italic),
            new PairedTagsParser("__", TagInfo.TagType.Strong),
            new EscapingParsing(),
            new LinkTagParser()
            });

        Console.WriteLine(parser.Render(s));
    }
}