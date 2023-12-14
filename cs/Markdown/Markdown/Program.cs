using Markdown.Parsers;
using Markdown.Tags.TagsContainers;

public class Program
{
    public static void Main(string[] args)
    {
        var text = "# Заголовок __с _разными_ символами__";
        var escapeCharacters = new HashSet<string>() { "\\" };
        var parser = new MarkdownParser(escapeCharacters, MarkdownTagsContainer.GetOpenTags());
        var htmlText = Markdown.Markdown.Render(text, parser);
        Console.WriteLine(htmlText);
    }
}