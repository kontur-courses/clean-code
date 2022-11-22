using Markdown.Converter;
using Markdown.Finder;

namespace Markdown;

public class MarkdownRenderer
{
    public string Render(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }
        var lines = text.Replace("\r\n", "\n").Split( "\n");
        var converter = new HtmlConverter();
        var markdownFinder = new MarkdownTagFinder();
        var htmlLines = lines.Select(line => converter.ConvertToHtml(line, markdownFinder.FindTags(line)));
        return string.Join("\n", htmlLines);
    }
}