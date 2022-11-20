using Markdown.Converter;
using Markdown.Indexer;

namespace Markdown;

public class MdRenderer
{
    public string Render(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var lines = text.Split("\n");
        var converter = new HtmlConverter();
        var mdFinder = new MdTagFinder();
        var htmlLines = lines.Select(line => converter.ConvertToMyMarkup(line, mdFinder.FindTags(line)));
        return string.Join("\n", htmlLines);
    }
}