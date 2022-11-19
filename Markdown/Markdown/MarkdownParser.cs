namespace Markdown;

public class MarkdownParser
{
    public static List<List<Token>> Parse(string text)
    {
        return text.Split('\n').Select(ParseLine).ToList();
    }

    public static List<Token> ParseLine(string line)
    {
        var tokens = new List<Token>();
        var lastStartIndex = 0;
        var markdownTags = MarkdownToHtmlConverter.MarkdownTags;
        for (var i = 0; i < line.Length; i++)
            foreach (var tag in markdownTags.Where(tag =>
                         i + tag.Key.Length <= line.Length && IsEqual(line, i, tag.Key.Length, tag.Key)))
            {
                if (lastStartIndex != i)
                    tokens.Add(new Token(line.Substring(lastStartIndex, i - lastStartIndex), TokenType.Text));
                tokens.Add(new Token(tag.Key, tag.Value));
                if (tag.Key.Length > 1)
                    i += tag.Key.Length - 1;
                lastStartIndex = i + 1;
                break;
            }

        if (lastStartIndex != line.Length)
            tokens.Add(new Token(line.Substring(lastStartIndex, line.Length - lastStartIndex), TokenType.Text));

        return tokens;
    }

    private static bool IsEqual(string line, int start, int length, string tag)
    {
        return line.IndexOf(tag, start, length, StringComparison.Ordinal) != -1;
    }
}