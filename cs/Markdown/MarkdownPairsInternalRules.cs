namespace Markdown;

internal static class MarkdownPairsInternalRules
{
    public static SpecialStringFormat DisapproveEmpty(this SpecialStringFormat lineFormat)
    {
        return lineFormat;
    }

    public static SpecialStringFormat DisapproveStartsOrEndsWithSpace(this SpecialStringFormat lineFormat)
    {
        return lineFormat;
    }

    public static SpecialStringFormat DisapproveWithDigits(this SpecialStringFormat lineFormat)
    {
        return lineFormat;
    }

    public static SpecialStringFormat DisapproveInDifferentWordParts(this SpecialStringFormat lineFormat)
    {
        return lineFormat;
    }
}