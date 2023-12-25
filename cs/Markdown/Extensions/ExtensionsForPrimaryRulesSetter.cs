namespace Markdown;

public class ExtensionsForPrimaryRulesSetter
{ 
    protected static TagAction[] InitializeActions(int length)
    {
        return Enumerable.Range(0, length)
            .Select(i => new TagAction(i))
            .ToArray();
    }

    protected static void HeadlineActions(TagAction[] actions, int lastIndex)
    {
        actions[0] = new TagAction(TypeTagAction.Open, 0, lastIndex - 1);
        actions[lastIndex - 1] = new TagAction(TypeTagAction.Close, lastIndex - 1, 0);
    }

    protected static void CursiveAction(TagAction[] actions, List<Tuple<int, int>> actionPairs, ref int openIndex, int currentIndex)
    {
        if (openIndex == -1)
        {
            openIndex = currentIndex;
        }
        else
        {
            actions[openIndex] = new TagAction(TypeTagAction.Open, openIndex, currentIndex);
            actions[currentIndex] = new TagAction(TypeTagAction.Close, currentIndex, openIndex);
            actionPairs.Add(new Tuple<int, int>(openIndex, currentIndex));
            openIndex = -1;
        }
    }

    protected static void BoldAction(TagAction[] actions, List<Tuple<int, int>> actionPairs, ref int openIndex, int currentIndex)
    {
        if (openIndex == -1)
        {
            openIndex = currentIndex;
        }
        else
        {
            actions[openIndex] = new TagAction(TypeTagAction.Open, openIndex, currentIndex);
            actions[currentIndex] = new TagAction(TypeTagAction.Close, currentIndex, openIndex);
            actionPairs.Add(new Tuple<int, int>(openIndex, currentIndex));
            openIndex = -1;
        }
    }

    protected static void InitializeActionNone(TagAction[] actions, int currentIndex)
    {
        actions[currentIndex] = new TagAction(currentIndex);
    }

    protected static void SetRemainingActionInvalid(TagAction[] actions, ref int openCursiveIndex, ref int openBoldIndex)
    {
        if (openCursiveIndex != -1)
        {
            actions[openCursiveIndex].IsValid = false;
        }
        
        if (openBoldIndex != -1)
        {
            actions[openBoldIndex].IsValid = false;
        }
    }
}