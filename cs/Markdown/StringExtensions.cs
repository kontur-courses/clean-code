using System.Text;
using Markdown.Tags;

namespace Markdown;

public static class StringExtensions
{
    public static bool IsTag(this string text, ITag tag, int idx, bool isOpen)
    {
        if (tag.GetType() == typeof(HeaderTag))
            return text.IsOpenOfParagraph(idx);

        if (idx == 0)
        {
            if (text.Length <= tag.Md.Length || (tag.GetType() == typeof(EmTag) && text[tag.Md.Length] == '_'))
                return false;

            return text[tag.Md.Length] != ' ';
        }

        if (text.Length <= idx + tag.Md.Length)
        {
            if (tag.GetType() == typeof(EmTag) && text[idx - 1] == '_')
                return false;

            if (isOpen)
                return false;

            return text[idx - 1] != ' ';
        }

        if (tag.GetType() == typeof(EmTag) && (text[idx - 1] == '_' || text[idx + tag.Md.Length] == '_'))
            return false;

        if (isOpen)
            return !char.IsLetter(text[idx - 1]) && !char.IsDigit(text[idx - 1]) && text[idx + tag.Md.Length] != ' ';

        return text[idx - 1] != ' '
               && !char.IsLetter(text[idx + tag.Md.Length]) && !char.IsDigit(text[idx + tag.Md.Length]);
    }

    public static bool IsShielded(this string text, int idx)
    {
        var count = 0;
        for (var i = idx - 1; i >= 0; i++)
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
                if (count % 2 == 1)
                    sb.Remove(i - 1 - count / 2, 1 + count / 2);
                else
                    sb.Remove(i - count / 2, count / 2);

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

    private static bool IsOpenOfParagraph(this string text, int idx)
    {
        return idx == 0 || text[idx - 1] == '\n';
    }

    public static int CloseIndexOfParagraph(this string text, int idx)
    {
        for (var i = idx + 1; i < text.Length; i++)
            if (text[idx] == '\n')
                return idx;

        return text.Length;
    }
}