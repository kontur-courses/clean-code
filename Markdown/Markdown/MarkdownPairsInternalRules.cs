namespace Markdown
{
    internal static class MarkdownPairsInternalRules
    {
        public static SpecialStringFormat DisapproveEmpty(this SpecialStringFormat lineFormat)
        {
            var actions = lineFormat.Actions;
            var line = lineFormat.ConvertedLine;

            for (int i = 0; i < line.Length; i++)
            {
                var act = actions[i];
                if (act.ActionType == MarkdownActionType.Open && act.Approved)
                {
                    if (act.PairIndex - act.SelfIndex == 1)
                    {
                        actions[i].Approved = false;
                        actions[actions[i].PairIndex].Approved = false;
                    }
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveStartsOrEndsWithSpace(this SpecialStringFormat lineFormat)
        {
            var actions = lineFormat.Actions;
            var line = lineFormat.ConvertedLine;

            for (int i = 0; i < line.Length; i++)
            {
                var act = actions[i];
                if (act.ActionType == MarkdownActionType.Open && act.Approved)
                {
                    if (line[act.SelfIndex + 1] == ' ' || line[act.PairIndex - 1] == ' ')
                    {
                        actions[i].Approved = false;
                        actions[actions[i].PairIndex].Approved = false;
                    }
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveWithDigits(this SpecialStringFormat lineFormat)
        {
            var openBrackets = new List<MarkdownAction>();
            var actions = lineFormat.Actions;
            var line = lineFormat.ConvertedLine;

            for (int i = 0; i < line.Length; i++)
            {
                var act = actions[i];
                if (act.ActionType == MarkdownActionType.Open && act.Approved)
                {
                    openBrackets.Add(act);
                }
                else if (act.ActionType == MarkdownActionType.Close && act.Approved)
                {
                    foreach (var ma in openBrackets)
                    {
                        if (ma.SelfIndex == act.PairIndex)
                        {
                            openBrackets.Remove(ma);
                            break;
                        }
                    }
                }
                else if (line[i] >= '0' && line[i] <= '9')
                {
                    foreach (var ma in openBrackets)
                    {
                        actions[ma.SelfIndex].Approved = false;
                        actions[ma.PairIndex].Approved = false;
                    }

                    openBrackets.Clear();
                }
            }

            return lineFormat;
        }
    }
}