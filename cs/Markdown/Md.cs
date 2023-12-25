using System.Text;

namespace Markdown;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Md.Render("_Trofimov Nikita_"));
    }
}
public static class Md
{
    public static string Render(string markdownText)
    {
        var convertedString = markdownText
            .Split('\n', '\r')
            .Select(ConvertMarkdownToHtml).ToList();

        return string.Join('\n', convertedString);
    }
    private static string ConvertMarkdownToHtml(string line)
    {
        var initialFormat = StringFormatter.GetStringFormat(line);
        var formattedWithPrimaryRules = PrimaryRulesSetter.SetPrimaryMarkdown(initialFormat);
        var сheckConditions = PairInteractionRules.HandleIntersectingPairs(formattedWithPrimaryRules)
            .CheckEmpty()
            .RemoveBoldInItalic()
            .CheckForDigits()
            .CheckInDifferentWordParts()
            .CheckStartOrEndWithSpace();
    
        return сheckConditions.ConvertToFormat();
    }
}
