namespace Markdown;

public class MarkdownParser
{
    public static List<List<TokenBase>> Parse(string text)
    {
        return text.Split('\n').Select(ParseLine).ToList();
    }

    public static List<TokenBase> ParseLine(string line)
    {
        return new List<TokenBase>();
    }

    private static bool IsEqual(string line, int start, int length, string tag)
    {
        return line.IndexOf(tag, start, length, StringComparison.Ordinal) != -1;
    }
}