using MarkdownTask.MarkdownParsers;

namespace MarkdownTask
{
    public static class Program
    {
        public static void Main()
        {
            var s = "# Header __with _different_ tags__";

            Markdown parser = new(new IMarkdownParser[]{
            new HeaderTagParser(),
            new PairedTagsParser("_", TagInfo.TagType.Italic),
            new PairedTagsParser("__", TagInfo.TagType.Strong),
            new EscapedCharactersParsing(),
            new LinkTagParser()
            });

            Console.WriteLine(parser.Render(s));
        }
    }
}