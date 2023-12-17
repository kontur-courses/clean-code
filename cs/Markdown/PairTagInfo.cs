namespace Markdown;

public class PairTagInfo((int openIdx, int closeIdx) tagIndexes)
{
    public (int openIdx, int closeIdx) TagIndexes { get; } = tagIndexes;
}