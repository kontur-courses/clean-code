using Markdown.Contracts;
using Markdown.Tags;

namespace Markdown.Helpers;

public static class TagHelper
{
    public static readonly char[] AvailableMarks = { '\\', '_', '#' };
    
    public static ITag? GetInstanceViaMark(string mark, int appearPosition)
    {
        if (mark.StartsWith("__"))
            return new BoldTag { ContainerPosition = appearPosition };

        if (mark.StartsWith("_"))
            return new ItalicTag { ContainerPosition = appearPosition };

        if (mark.StartsWith("# "))
            return new HeaderTag { ContainerPosition = appearPosition };

        if (mark.StartsWith("\\n"))
            return new NewlineTag { ContainerPosition = appearPosition };

        return null;
    }

    // Don't repeat same logic for underscore (_, __, ___, ... ) tags
    public static bool IsUnderscoreTagBroken(string context, ITag tag)
    {
        var position = tag.ContainerPosition;
        var offset = tag.Info.GlobalMark.Length;
        
        var check1 = position == 0 && char.IsWhiteSpace(context[position + offset]);
        var check2 = position >= context.Length - 2 && char.IsWhiteSpace(context[position - 1]);

        if (check1 || check2)
            return true;

        var (previousChar, nextChar) = (context[position - 1], context[position + offset]);

        var check3 = char.IsWhiteSpace(previousChar) && char.IsWhiteSpace(nextChar);
        var check4 = char.IsDigit(previousChar) && char.IsDigit(nextChar);

        return check3 || check4;
    }
}