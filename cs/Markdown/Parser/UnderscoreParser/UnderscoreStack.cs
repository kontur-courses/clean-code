namespace Markdown;

public class UnderscoreStack
{
    private readonly List<Underscore> items = [];
    public int SingleDepth { get; private set; }
    public int DoubleDepth { get; private set; }

    public void Push(bool isStrong, int tokenIndex, int nodeIndex, int contentStartIndex, bool suppressStrong)
    {
        var Underscore = new Underscore(isStrong, tokenIndex, nodeIndex, contentStartIndex, suppressStrong);
        items.Add(Underscore);

        if (!isStrong)
        {
            SingleDepth++;
            return;
        }

        if (!suppressStrong)
            DoubleDepth++;
    }

    public UnderscoreMatch? FindMatchingUnderscore(bool isStrong, int closingTokenIndex, UnderscoreValidator validator)
    {
        Underscore? blockingUnderscore = null;

        for (var i = items.Count - 1; i >= 0; i--)
        {
            var opener = items[i];

            if (opener.IsStrong != isStrong)
            {
                blockingUnderscore ??= opener;
                continue;
            }

            if (blockingUnderscore != null &&
                validator.HasClosingUnderscoreAhead(blockingUnderscore, closingTokenIndex))
            {
                return new UnderscoreMatch(null, -1, true);
            }

            return new UnderscoreMatch(opener, i, false);
        }

        return null;
    }

    public void RemoveFrom(int indexInclusive)
    {
        for (var i = items.Count - 1; i >= indexInclusive; i--)
            RemoveAt(i);
    }

    public void RemoveAt(int index)
    {
        var Underscore = items[index];

        if (!Underscore.IsStrong)
            SingleDepth--;
        else if (!Underscore.IsSuppressed)
            DoubleDepth--;

        items.RemoveAt(index);
    }
}