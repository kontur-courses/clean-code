namespace Markdown
{
    internal static class PrimaryMarkdownMaker
    {
        public static SpecialStringFormat SetPrimaryMarkdown(this SpecialStringFormat specialStringFormat)
        {
            var convertedLine = specialStringFormat.ConvertedLine;
            var operationalCharacters = specialStringFormat.OperationalCharacters;
            var Actions = new MarkdownAction[convertedLine.Length];

            for (int i = 0; i < convertedLine.Length; i++)
            {
                if (operationalCharacters[i] && Actions[i].ActionType == MarkdownActionType.None)
                {
                    int indx = FindPairAction(convertedLine, i);
                    if (indx == -1)
                    {
                        Actions[i] = new MarkdownAction(i);
                        Actions[i].Approved = false;
                    }
                    else
                    {
                        Actions[i] = new MarkdownAction(MarkdownActionType.Open, i, indx);
                        Actions[indx] = new MarkdownAction(MarkdownActionType.Close, indx, i);


                        Actions[i] = new MarkdownAction(MarkdownActionType.Open, i, indx);
                        Actions[indx] = new MarkdownAction(MarkdownActionType.Close, indx, i);
                    }
                }
                else if (Actions[i].ActionType == MarkdownActionType.None)
                {
                    Actions[i] = new MarkdownAction(i);
                }
            }

            specialStringFormat.Actions = Actions;
            return specialStringFormat;
        }

        private static int FindPairAction(string specialString, int startIndex)
        {
            for (int i = startIndex + 1; i < specialString.Length; i++)
            {
                if (specialString[i] == specialString[startIndex]) return i;
            }

            return -1;
        }
    }
}