namespace Markdown
{
    internal static class MarkdownPairsInternalRules
    {
        public static SpecialStringFormat DisapproveEmpty(this SpecialStringFormat lineFormat)
        {
            var line = lineFormat.ConvertedLine;
            var actions = lineFormat.Actions;

            foreach (var pair in lineFormat.ActionPairs)
            {
                if (!actions[pair.Item1].Approved) continue;
                if (pair.Item1 == pair.Item2 - 1)
                {
                    actions[pair.Item1].Approved = false;
                    actions[pair.Item2].Approved = false;
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveStartsOrEndsWithSpace(this SpecialStringFormat lineFormat)
        {
            var line = lineFormat.ConvertedLine;
            var actions = lineFormat.Actions;

            foreach (var pair in lineFormat.ActionPairs)
            {
                if (!actions[pair.Item1].Approved) continue;
                if (line[pair.Item1 + 1] == ' ' || line[pair.Item2 - 1] == ' ')
                {
                    actions[pair.Item1].Approved = false;
                    actions[pair.Item2].Approved = false;
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveWithDigits(this SpecialStringFormat lineFormat)
        {
            var line = lineFormat.ConvertedLine;
            var actions = lineFormat.Actions;

            foreach (var pair in lineFormat.ActionPairs)
            {
                if (!actions[pair.Item1].Approved) continue;
                for (int i = pair.Item1; i <= pair.Item2; i++)
                {
                    if (line[i] >= '0' && line[i] <= '9')
                    {
                        actions[pair.Item1].Approved = false;
                        actions[pair.Item2].Approved = false;
                        break;
                    }
                }
            }

            return lineFormat;
        }

        public static SpecialStringFormat DisapproveInDifferentWordParts(this SpecialStringFormat lineFormat)
        {
            var line = lineFormat.ConvertedLine;
            var actions = lineFormat.Actions;

            foreach (var pair in lineFormat.ActionPairs)
            {
                if (!actions[pair.Item1].Approved) continue;
                if ((pair.Item1 != 0 && line[pair.Item1 - 1] != ' ' && line[pair.Item1 - 1] != '#') ||
                    (pair.Item2 != line.Length - 1 && line[pair.Item2 + 1] != ' ' && line[pair.Item2 + 1] != '#'))
                {
                    for (int i = pair.Item1; i <= pair.Item2; i++)
                    {
                        if (line[i] == ' ')
                        {
                            actions[pair.Item1].Approved = false;
                            actions[pair.Item2].Approved = false;
                            break;
                        }
                    }
                }
            }

            return lineFormat;
        }
    }
}