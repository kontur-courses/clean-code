using Markdown.Converter;
using Markdown.Indexer;

namespace Markdown;

public class Md
{
    public string Render(string text)
    {
        if (text == null)
        {
            throw new ArgumentException("text can't be null");
        }

        var lines = text.Split("\n");
        var converter = new HtmlConverter();
        var indexer = new TagIndexer();
        var htmlLines = lines.Select(line => converter.Convert(line, indexer.IndexTags(line)));
        return string.Join("\n", htmlLines);
    }
}