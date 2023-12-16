using System.Text;

namespace Markdown;

public class Md
{
    public static string Reader(string MarkdownText)
    {
        var line = MarkdownText.Split('\n', '\r');
        var convertedLines = line.Select(t => ConvertedLines(t)).ToList();

        return string.Join('\n', convertedLines);

        string ConvertedLines(string line)
        {
            var stringFormat = ConvertedToHtml.getStringFormat(line);
            return stringFormat.ConvertFormat();
        }
    }
}