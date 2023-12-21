using MarkDown.Interfaces;
using MarkDown.TagContexts.Abstracts;

namespace MarkDown.ContextChanges;

public class ContextChange : IContextChange
{
    public ContextChange(bool hasChanges, TagContext newContext, int skipIndexesCount)
    {
        HasChanges = hasChanges;
        NewContext = newContext;
        SkipIndexesCount = skipIndexesCount;
    }

    public bool HasChanges { get; }
    public TagContext NewContext { get; }
    public int SkipIndexesCount { get; }
}