namespace Markdown;

public class TagAction
{
    public TypeTagAction ActionType { get; private set; }
    public int Index { get; }
    public int PairIndex { get; }
    public bool IsValid = true;

    public TagAction(TypeTagAction tagAction, int index, int pairIndex)
    {
        ActionType = tagAction;
        Index = index;
        PairIndex = pairIndex;
    }

    public TagAction(int index = -1)
    {
        ActionType = TypeTagAction.None;
        Index = index;
    }
}