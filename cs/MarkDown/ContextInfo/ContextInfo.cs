using MarkDown.Interfaces;
using MarkDown.TagContexts;

namespace MarkDown.ContextInfo;

public class ContextInfo : IContextInfo
{
    private readonly List<int> screeningIndexes;
    
    public ContextInfo(EntryContext entryContext, List<int> screeningIndexes)
    {
        EntryContext = entryContext;
        this.screeningIndexes = screeningIndexes;
    }

    public EntryContext EntryContext { get; }
    public IReadOnlyCollection<int> ScreeningIndexes => screeningIndexes;
}