namespace Markdown
{
    internal class PrimaryRulesSetter : ExtensionsForPrimaryRulesSetter
    {
        public static StringFormatter SetPrimaryMarkdown(StringFormatter specialStringFormat)
        {
            var newFormat = new StringFormatter(specialStringFormat);
            var convertedLine = newFormat.FormattedString;
            var operationalCharacters = newFormat.SpecialCharacters;
            var actions = InitializeActions(convertedLine.Length);
            var actionPairs = newFormat.ActionPairs = new List<Tuple<int, int>>();

            var openCursiveIndex = -1;
            var openBoldIndex = -1;

            if (convertedLine[0] == '#' && operationalCharacters[0])
            {
                HeadlineActions(actions, convertedLine.Length);
            }

            for (var i = 0; i < convertedLine.Length; i++)
            {
                if (operationalCharacters[i] && convertedLine[i] == '_')
                {
                    CursiveAction(actions, actionPairs, ref openCursiveIndex, i);
                }
                else if (operationalCharacters[i] && convertedLine[i] == ';')
                {
                    BoldAction(actions, actionPairs, ref openBoldIndex, i);
                }
                else if (actions[i].ActionType == TypeTagAction.None)
                {
                    InitializeActionNone(actions, i);
                }
            }

            SetRemainingActionInvalid(actions, ref openCursiveIndex, ref openBoldIndex);

            newFormat.Actions = actions;
            return newFormat;
        }
    }
}