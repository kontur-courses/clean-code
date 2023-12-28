using System.Text;

namespace Markdown;

public static class StringExtensions
{
    public static bool IsShielded(this string text, int idx)
    {
        var count = 0;
        for (var i = idx - 1; i >= 0; i--)
        {
            if (text[i] == '\\')
                count++;
            else
                break;
        }

        return count % 2 == 1;
    }

    public static string ReplaceShieldSequences(this string text)
    {
        var sb = new StringBuilder(text);

        var count = 0;
        for (var i = 0; i < sb.Length; i++)
        {
            if (sb[i] == '\\')
                count++;
            else if (count != 0)
            {
                sb.Remove(i - count / 2, count / 2);
                if (count % 2 == 1)
                    sb.Remove(i - 1 - count / 2, 1);

                count = 0;
            }
        }

        if (count != 0)
        {
            sb.Remove(sb.Length - 1 - count / 2, count / 2);
            if (count % 2 == 1)
                sb.Remove(sb.Length - 1, 1);
        }

        return sb.ToString();
    }

    public static bool IsOpenOfParagraph(this string text, int idx)
    {
        return idx == 0 || text[idx - 1] == '\n';
    }
}