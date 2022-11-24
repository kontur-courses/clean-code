using System.Text;

namespace Markdown;

public class Md
{
    public string Render(string text)
    {
        var result = new StringBuilder();

        var i = 0;
        var lines = text.Split('\n');
        var converter = new MarkdownToHtmlConverter();
        foreach (var line in lines)
        {
            var tokens = MarkdownParser.ParseLine(line);
            result.Append(converter.ToHtml(line, tokens));
            if (i++ != lines.Length - 1)
                result.Append('\n');
        }

        return result.ToString();
    }
}