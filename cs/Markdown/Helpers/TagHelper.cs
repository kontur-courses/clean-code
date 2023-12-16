using Markdown.Contracts;
using Markdown.Tags;

namespace Markdown.Helpers;

public static class TagHelper
{
    public static readonly char[] AvailableMarks = { '\\', '_', '#' };
    
    public static ITag? GetInstanceViaMark(string mark)
    {
        if (mark.StartsWith("__"))
            return new BoldTag();

        if (mark.StartsWith("_"))
            return new ItalicTag();

        if (mark.StartsWith("# "))
            return new HeaderTag();

        if (mark.StartsWith("\\n"))
            return new NewlineTag();

        return null;
    }

    public static bool IsUnderscoreTagBroken(ITag tag)
    {
        var check1 = char.IsWhiteSpace(tag.PreviousChar) && char.IsWhiteSpace(tag.NextChar);
        var check2 = char.IsDigit(tag.PreviousChar) && char.IsDigit(tag.NextChar);
        return check1 || check2;
    }
}