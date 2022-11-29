namespace Markdown;

internal static class MarkdownPairsInteractionRules
{
    public static void DisapproveIntersectingPairs(SpecialStringFormat lineFormat)
    {
    }

    public static SpecialStringFormat DisapproveBoldInCursive(this SpecialStringFormat lineFormat)
    {
        return lineFormat;
    }
}