using MarkDown.TagContexts;

namespace MarkDown.Interfaces;

public interface IContextInfo
{
    public EntryContext EntryContext { get; }
    public IReadOnlyCollection<int> ScreeningIndexes { get; }
}