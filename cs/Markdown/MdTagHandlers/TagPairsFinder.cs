namespace Markdown.MdTagHandlers;

internal class TagPairsFinder : ITagPairsFinder
{
    public IEnumerable<TagReplacementsPair> PairTags(
        string str,
        IEnumerable<SubstringReplacement> tagPositions)
    {
        var open = default(SubstringReplacement);

        foreach (var current in tagPositions)
        {
            var canBeOpenTag = current.EndIndex != str.Length && !char.IsWhiteSpace(str[current.EndIndex]);
            var canBeCloseTag = current.Index != 0 && !char.IsWhiteSpace(str[current.Index - 1]);

            if (open == default)
            {
                if (canBeOpenTag)
                {
                    open = current;
                }
            }
            else if (canBeCloseTag
                && current.Index != open.EndIndex
                && (!canBeOpenTag || AreInSameWord(str, open, current)))
            {
                yield return new TagReplacementsPair(open, current);
                open = default;
            }
            else if (canBeOpenTag)
            {
                open = current;
            }
        }
    }

    public IEnumerable<TagReplacementsPair> PairFullLineTags(
        string str,
        IEnumerable<SubstringReplacement> tagPositions)
    {
        return PairFullLineTags(GetLineBreakIndices(str), tagPositions.ToArray());
    }

    private bool AreInSameWord(string str, SubstringReplacement open, SubstringReplacement close)
    {
        for (var i = open.EndIndex; i < close.Index; i++)
        {
            if (char.IsWhiteSpace(str[i]))
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerable<TagReplacementsPair> PairFullLineTags(
        int[] lineBreakIndices,
        SubstringReplacement[] tagPositions)
    {
        for (var (i, j) = (0, 0); i < tagPositions.Length; i++)
        {
            while (lineBreakIndices[j] < tagPositions[i].EndIndex)
            {
                j++;
            }

            yield return new TagReplacementsPair(tagPositions[i], new SubstringReplacement(lineBreakIndices[j], 0));
        }
    }

    private int[] GetLineBreakIndices(string str)
    {
        return Enumerable
            .Range(0, str.Length)
            .Where(i => str[i] == '\n')
            .Append(str.Length)
            .ToArray();
    }
}
