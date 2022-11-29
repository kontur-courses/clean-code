namespace Markdown;

internal static class PrimaryMarkdownMaker
{
    public static SpecialStringFormat SetPrimaryMarkdown(SpecialStringFormat specialStringFormat)
    {
        var newFormat = new SpecialStringFormat(specialStringFormat);
        var convertedLine = newFormat.ConvertedLine;
        var operationalCharacters = newFormat.OperationalCharacters;
        var actions = new MarkdownAction[convertedLine.Length];
        var actionPairs = newFormat.ActionPairs = new List<Tuple<int, int>>();

        int openCursiveIndex = -1;
        int openBoldIndex = -1;

        if (convertedLine[0] == '#' && operationalCharacters[0])
        {
            actions[0] = new MarkdownAction(MarkdownActionType.Open, 0, convertedLine.Length - 1);
            actions[^1] = new MarkdownAction(MarkdownActionType.Close, convertedLine.Length - 1, 0);
        }

        for (int i = 0; i < convertedLine.Length; i++)
        {
            if(actions[i] == null)
                actions[i] = new MarkdownAction(i);
            if (operationalCharacters[i] && actions[i].ActionType == MarkdownActionType.None)
            {
                if (convertedLine[i] == '_')
                {
                    if (openCursiveIndex == -1) openCursiveIndex = i;
                    else
                    {
                        actions[openCursiveIndex] =
                            new MarkdownAction(MarkdownActionType.Open, openCursiveIndex, i);
                        actions[i] =
                            new MarkdownAction(MarkdownActionType.Close, i, openCursiveIndex);
                        actionPairs.Add(new Tuple<int, int>(openCursiveIndex, i));
                        openCursiveIndex = -1;
                    }
                }

                if (convertedLine[i] == ';')
                {
                    if (openBoldIndex == -1) openBoldIndex = i;
                    else
                    {
                        actions[openBoldIndex] =
                            new MarkdownAction(MarkdownActionType.Open, openBoldIndex, i);
                        actions[i] =
                            new MarkdownAction(MarkdownActionType.Close, i, openBoldIndex);
                        actionPairs.Add(new Tuple<int, int>(openBoldIndex, i));
                        openBoldIndex = -1;
                    }
                }
            }
            else if (actions[i].ActionType == MarkdownActionType.None)
            {
                actions[i] = new MarkdownAction(i);
            }
        }

        if (openCursiveIndex != -1)
        {
            actions[openCursiveIndex] = new MarkdownAction(openCursiveIndex);
            actions[openCursiveIndex].Approved = false;
        }

        if (openBoldIndex != -1)
        {
            actions[openBoldIndex] = new MarkdownAction(openBoldIndex);
            actions[openBoldIndex].Approved = false;
        }

        newFormat.Actions = actions;
        return newFormat;
    }
}