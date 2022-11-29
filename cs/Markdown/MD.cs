using System.Text;

namespace Markdown;

public class Md
{

    public static string Render(string MarkdownText)
    {
        var line = MarkdownText.Split('\n', '\r');
        var convertedLines = new List<string>();
        for (int i = 0; i < line.Length; i++)
        {
            convertedLines.Add(ConvertLine(line[i]));
        }

        return string.Join('\n', convertedLines);
    }

    private static string ConvertLine(string line)
    {
        var specialStringFormat = new SpecialStringFormat(line);
        PrimaryMarkdownMaker.SetPrimaryMarkdown(specialStringFormat);
        MarkdownPairsInteractionRules.DisapproveIntersectingPairs(specialStringFormat);
        specialStringFormat.DisapproveEmpty()
            .DisapproveBoldInCursive()
            .DisapproveWithDigits()
            .DisapproveInDifferentWordParts()
            .DisapproveStartsOrEndsWithSpace();
        return specialStringFormat.ConvertFromFormat();
    }
}