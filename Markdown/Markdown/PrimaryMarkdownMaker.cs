namespace Markdown
{
    internal static class PrimaryMarkdownMaker
    {
        public static SpecialStringFormat SetPrimaryMarkdown(this SpecialStringFormat specialStringFormat)
        {
            var convertedLine = specialStringFormat.ConvertedLine;
            var operationalCharacters = specialStringFormat.OperationalCharacters;
            var Actions = new MarkdownAction[convertedLine.Length];
            var ActionPairs = specialStringFormat.ActionPairs = new List<Tuple<int, int>>();

            int openCursiveIndex = -1;
            int openBoldIndex = -1;

            if (convertedLine[0] == '#' && operationalCharacters[0])
            {
                Actions[0] = new MarkdownAction(MarkdownActionType.Open, 0, convertedLine.Length - 1);
                Actions[^1] = new MarkdownAction(MarkdownActionType.Close, convertedLine.Length - 1, 0);
            }

            for (int i = 0; i < convertedLine.Length; i++)
            {
                if (operationalCharacters[i] && Actions[i].ActionType == MarkdownActionType.None)
                {
                    if (convertedLine[i] == '_')
                    {
                        if (openCursiveIndex == -1) openCursiveIndex = i;
                        else
                        {
                            Actions[openCursiveIndex] =
                                new MarkdownAction(MarkdownActionType.Open, openCursiveIndex, i);
                            Actions[i] =
                                new MarkdownAction(MarkdownActionType.Close, i, openCursiveIndex);
                            ActionPairs.Add(new Tuple<int, int>(openCursiveIndex, i));
                            openCursiveIndex = -1;
                        }
                    }

                    if (convertedLine[i] == ';')
                    {
                        if (openBoldIndex == -1) openBoldIndex = i;
                        else
                        {
                            Actions[openBoldIndex] =
                                new MarkdownAction(MarkdownActionType.Open, openBoldIndex, i);
                            Actions[i] =
                                new MarkdownAction(MarkdownActionType.Close, i, openBoldIndex);
                            ActionPairs.Add(new Tuple<int, int>(openBoldIndex, i));
                            openBoldIndex = -1;
                        }
                    }
                }
                else if (Actions[i].ActionType == MarkdownActionType.None)
                {
                    Actions[i] = new MarkdownAction(i);
                }
            }

            if (openCursiveIndex != -1)
            {
                Actions[openCursiveIndex] = new MarkdownAction(openCursiveIndex);
                Actions[openCursiveIndex].Approved = false;
            }

            if (openBoldIndex != -1)
            {
                Actions[openBoldIndex] = new MarkdownAction(openBoldIndex);
                Actions[openBoldIndex].Approved = false;
            }

            specialStringFormat.Actions = Actions;
            return specialStringFormat;
        }
    }
}