using Markdown.TokenConverter;

namespace Markdown.Renderer;

public static class OuterTagsProcessor
{
    public static IReadOnlyList<int> GetPositionsWithOuterTags(List<TokenConversionResult> lines)
    {
        var outerTags = new List<int>();
        var firstIndex = -1;
        for (var i = 0; i < lines.Count; i++)
        {
            if (firstIndex == -1 && lines[i].OuterTag is not null)
            {
                firstIndex = i;
            }

            if (firstIndex == -1)
                continue;

            if (lines[i].OuterTag is not null && i == lines.Count - 1)
            {
                outerTags.Add(firstIndex);
                outerTags.Add(i);
                return outerTags;
            }

            if (lines[i].OuterTag is null && i > 0 && lines[i - 1].OuterTag is not null)
            {
                outerTags.Add(firstIndex);
                outerTags.Add(i - 1);
                firstIndex = -1;
            }
        }

        return outerTags;
    }
}