namespace Markdown
{
    public static class PairInteractionRules
    {
        public static StringFormatter HandleIntersectingPairs(StringFormatter stringFormatter)
        {
            var updatedFormatter = new StringFormatter(stringFormatter);
            var openTagActions = new List<TagAction>();
            var actions = updatedFormatter.Actions;
        
            foreach (var action in actions)
            {
                switch (action.ActionType)
                {
                    case TypeTagAction.Open:
                        openTagActions.Add(action);
                        break;
                    case TypeTagAction.Close when openTagActions[^1].PairIndex == action.Index:
                        openTagActions.RemoveAt(openTagActions.Count - 1);
                        break;
                    case TypeTagAction.Close:
                        actions[openTagActions[^1].Index].IsValid = false;
                        actions[openTagActions[^1].PairIndex].IsValid = false;
                        actions[openTagActions[0].Index].IsValid = false;
                        actions[openTagActions[0].PairIndex].IsValid = false;

                        openTagActions.RemoveAt(0);
                        break;
                }
            }
        
            return updatedFormatter;
        }
        
        public static StringFormatter RemoveBoldInItalic(this StringFormatter lineFormatter)
        {
            var line = lineFormatter.FormattedString;
            var actions = lineFormatter.Actions;
        
            foreach (var pair in lineFormatter.ActionPairs
                         .Where(pair => actions[pair.Item1].IsValid && line[pair.Item1] == '_'))
            {
                lineFormatter = HandleBoldInItalic(lineFormatter, pair);
            }
        
            return lineFormatter;
        }

        private static StringFormatter HandleBoldInItalic(StringFormatter lineFormatter, Tuple<int, int> pair)
        {
            var updatedFormatter = new StringFormatter(lineFormatter);
            var line = updatedFormatter.FormattedString;
            var actions = updatedFormatter.Actions;

            for (var i = pair.Item1; i <= pair.Item2; i++)
            {
                if (actions[i].ActionType != TypeTagAction.Open || !actions[i].IsValid || line[i] != ';') continue;
                actions[i].IsValid = false;
                actions[actions[i].PairIndex].IsValid = false;
                break;
            }

            return updatedFormatter;
        }
    }
}
