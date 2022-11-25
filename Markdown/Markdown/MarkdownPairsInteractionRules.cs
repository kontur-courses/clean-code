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
            var line = lineFormat.ConvertedLine;
            var actions = lineFormat.Actions;

            foreach (var pair in lineFormat.ActionPairs)
            {
                if (!actions[pair.Item1].Approved || line[pair.Item1] != '_') continue;
                for (int i = pair.Item1; i <= pair.Item2; i++)
                {
                    if (actions[i].ActionType == MarkdownActionType.Open && actions[i].Approved && line[i] == ';')
                    {
                        actions[i].Approved = false;
                        actions[actions[i].PairIndex].Approved = false;
                        break;
                    }
                }
            }

            return lineFormat;
        }
    }
}