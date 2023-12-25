using System.Diagnostics;

namespace Markdown;

internal static class ConversionRestrictions
{
    public static StringFormatter CheckEmpty(this StringFormatter lineFormatter)
    {
        var actions = lineFormatter.Actions;

        lineFormatter.ActionPairs
            .Where(pair => actions[pair.Item1].IsValid
                           && pair.Item1 == pair.Item2 - 1)
            .ToList()
            .ForEach(pair =>
            {
                ActionsIsValidFalse(actions, pair.Item1, pair.Item2);
            });

        return lineFormatter;
    }

    public static StringFormatter CheckStartOrEndWithSpace(this StringFormatter lineFormatter)
    {
        var line = lineFormatter.FormattedString;
        var actions = lineFormatter.Actions;

        foreach (var pair in lineFormatter.ActionPairs)
        {
            if (!actions[pair.Item1].IsValid) continue;
            if (line[pair.Item1 + 1] != ' ' && line[pair.Item2 - 1] != ' ') continue;
            ActionsIsValidFalse(actions, pair.Item1, pair.Item2);
        }

        return lineFormatter;
    }

    public static StringFormatter CheckForDigits(this StringFormatter lineFormatter)
    {
        var line = lineFormatter.FormattedString;
        var actions = lineFormatter.Actions;

        foreach (var pair in lineFormatter.ActionPairs)
        {
            if (!actions[pair.Item1].IsValid) continue;
            for (var i = pair.Item1; i <= pair.Item2; i++)
            {
                if (line[i] < '0' || line[i] > '9') continue;
                ActionsIsValidFalse(actions, pair.Item1, pair.Item2);
                break;
            }
        }

        return lineFormatter;
    }

    public static StringFormatter CheckInDifferentWordParts(this StringFormatter lineFormatter)
    {
        var line = lineFormatter.FormattedString;
        var actions = lineFormatter.Actions;

        foreach (var pair in lineFormatter.ActionPairs)
        {
            if (!actions[pair.Item1].IsValid) continue;
            if ((pair.Item1 == 0 || line[pair.Item1 - 1] == ' ' || line[pair.Item1 - 1] == '#') &&
                (pair.Item2 == line.Length - 1 || line[pair.Item2 + 1] == ' ' ||
                 line[pair.Item2 + 1] == '#')) continue;
            for (var i = pair.Item1; i <= pair.Item2; i++)
            {
                if (line[i] != ' ') continue;
                ActionsIsValidFalse(actions, pair.Item1, pair.Item2);
                break;
            }
        }
        return lineFormatter;
    }

    private static void ActionsIsValidFalse(TagAction[] actions, int item1, int item2)
    {
        actions[item1].IsValid = false;
        actions[item2].IsValid = false;        
    }
}