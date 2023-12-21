using MarkDown.Interfaces;

namespace MarkDown.ContextChanges;

public class ScreeningChange : IContextChange
{
    public ScreeningChange(bool hasChanges, bool isScreened, bool needToAddIndex)
    {
        HasChanges = hasChanges;
        IsScreened = isScreened;
        NeedToAddIndex = needToAddIndex;
    }

    public bool HasChanges { get; }
    public bool NeedToAddIndex { get; }
    public bool IsScreened { get; }
}