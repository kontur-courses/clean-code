namespace Markdown;

internal class MarkdownAction
{
    public MarkdownActionType ActionType { get; private set; }
    public int SelfIndex { get; private set; } = -1;
    public int PairIndex { get; private set; } = -1;

    public bool Approved = true;

    public MarkdownAction(MarkdownActionType action, int index, int pairIndex)
    {
        ActionType = action;
        SelfIndex = index;
        PairIndex = pairIndex;
    }

    public MarkdownAction(int index = -1)
    {
        ActionType = MarkdownActionType.None;
        SelfIndex = index;
    }
}