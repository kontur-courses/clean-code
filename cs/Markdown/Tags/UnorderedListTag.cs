using System.Text;
using Markdown.Interfaces;
using Markdown.Parsers;

namespace Markdown.Tags;

public sealed class UnorderedListTag : IBlockTag
{
    public bool CanHandle(string blockText)
    {
        if (blockText.Length == 0)
            return false;

        foreach (var line in blockText.Split('\n'))
        {
            if (!line.StartsWith("- "))
                return false;
        }

        return true;
    }

    public string Render(string blockText, InlineParser inlineParser)
    {
        var result = new StringBuilder();
        result.Append("<ul>");
        foreach (var line in blockText.Split('\n'))
        {
            result.Append("<li>");
            result.Append(inlineParser.Parse(line[2..]));
            result.Append("</li>");
        }

        result.Append("</ul>");
        return result.ToString();
    }
}