namespace Markdown;

internal record struct SubstringReplacement(int Index, int Length, string? Replacement = null)
{
    public readonly int EndIndex => Index + Length;
}
