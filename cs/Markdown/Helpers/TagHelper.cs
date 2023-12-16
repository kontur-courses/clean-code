using Markdown.Contracts;

namespace Markdown.Helpers;

public static class TagHelper
{
    public static bool IsUnderscoreTagBroken(ITag tag)
    {
        var check1 = char.IsWhiteSpace(tag.PreviousChar) && char.IsWhiteSpace(tag.NextChar);
        var check2 = char.IsDigit(tag.PreviousChar) && char.IsDigit(tag.NextChar);
        return check1 || check2;
    }
}