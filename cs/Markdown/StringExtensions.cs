using Markdown.Tags;

namespace Markdown;

public static class StringExtensions
{
    public static bool IsSingleTag(this string text, ITag tag, int idx)
    {
        return tag.GetType() == typeof(HeaderTag) && text.IsOpenOfParagraph(idx);
    }

    public static bool IsPairTag(this string text, ITag tag, int idx, bool isOpen)
    {
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
        throw new NotImplementedException();
    }

    private static bool IsOpenOfParagraph(this string text, int idx)
    {
        return idx == 0 || text[idx - 1] == '\n';
    }

    public static int CloseIndexOfParagraph(this string text, int idx)
    {
        for (var i = idx; i < text.Length; i++)
            if (text[idx] == '\n')
                return idx;

        return -1;
    }
}