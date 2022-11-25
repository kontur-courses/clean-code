namespace Markdown
{
    internal static class MarkdownPairsInteractionRules
    {
        public static SpecialStringFormat DisapproveIntersectingPairs(this SpecialStringFormat lineFormat)
        {
            var openBrackets = new List<MarkdownAction>();
            var actions = lineFormat.Actions;
            var line = lineFormat.ConvertedLine;

            for (int i = 0; i < line.Length; i++)
            {
                var act = actions[i];
                if (act.ActionType == MarkdownActionType.Close)
                {
                    if (openBrackets[^1].PairIndex == act.SelfIndex)
                    {
                        openBrackets.RemoveAt(openBrackets.Count - 1);
                    }
                    else
                    {
                        actions[openBrackets[^1].SelfIndex].Approved = false;
                        actions[openBrackets[^1].PairIndex].Approved = false;
                        actions[openBrackets[0].SelfIndex].Approved = false;
                        actions[openBrackets[0].PairIndex].Approved = false;

                        openBrackets.RemoveAt(0);
                    }
                }
                else if (act.ActionType == MarkdownActionType.Open)
                {
                    openBrackets.Add(act);
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveBoldInCursive(this SpecialStringFormat lineFormat)
        {
            var openBrackets = new List<MarkdownAction>();
            var actions = lineFormat.Actions;
            var line = lineFormat.ConvertedLine;

            for (int i = 0; i < line.Length; i++)
            {
                var act = actions[i];


                if (act.ActionType == MarkdownActionType.Open && act.Approved)
                {
                    if (openBrackets.Count != 0 && line[i] == ';')
                    {
                        actions[act.SelfIndex].Approved = false;
                        actions[act.PairIndex].Approved = false;
                    }
                    else
                    {
                        openBrackets.Add(act);
                    }
                }
                else if (act.ActionType == MarkdownActionType.Close && act.Approved)
                {
                    openBrackets.RemoveAt(openBrackets.Count - 1);
                }
            }

            return lineFormat;
        }
    }
}