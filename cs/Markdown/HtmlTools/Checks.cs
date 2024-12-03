using Markdown.CandidateInfo;

namespace Markdown.HtmlTools;

internal static class Checks
{
    private const char EscapeChar = '\\';

    public static bool IsEscaped(string text, int position)
    {
        if (position <= 0 || position >= text.Length)
        {
            return false;
        }

        var escapingCharCount = 0;

        while (position > 0)
        {
            if (text[position - 1] == EscapeChar)
            {
                escapingCharCount++;
                position--;
            }
            else
            {
                break;
            }
        }

        return escapingCharCount % 2 != 0;
    }

    public static bool IsAfterSpace(string text, int position)
    {
        if (position < 0 || position > text.Length)
        {
            return false;
        }

        return position == 0 || char.IsWhiteSpace(text[position - 1]);
    }

    public static bool IsBeforeSpace(string text, int position)
    {
        if (position < 0 || position >= text.Length)
        {
            return false;
        }

        return position == text.Length - 1 || char.IsWhiteSpace(text[position + 1]);
    }

    public static bool CanSelect(string text, Candidate open, Candidate close)
    {
        if (close.Position - open.Position == 1)
        {
            return true;
        }

        var content = text.Substring(open.Position, close.Position - open.Position);
        var isContentWithDigits = IsContentWithDigits(content);

        if (isContentWithDigits)
        {
            return false;
        }

        if (open.EdgeType == EdgeType.Edge && close.EdgeType == EdgeType.Edge)
        {
            return true;
        }

        var oneWord = content.Split(' ').Length == 1;

        return oneWord;
    }

    private static bool IsContentWithDigits(string word) => word.Any(char.IsDigit);
}