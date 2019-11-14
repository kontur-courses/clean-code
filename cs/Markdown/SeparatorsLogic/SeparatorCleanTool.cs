using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class SeparatorCleanTool
    {
        public static List<Separator> GetCorrectSeparators(string input, Dictionary<string, List<Separator>> separators)
        {
            var sortedSeparators = separators.Values
                .SelectMany(x => x)
                .OrderBy(x => x.Index)
                .ToList();

            return sortedSeparators
                .RemoveShieldedDelimiters(input)
                .RemoveAllUnclosedSeparators()
                .RemoveIncorrectNestingSeparators()
                .ToList();
        }

        static IEnumerable<Separator> RemoveShieldedDelimiters(this IEnumerable<Separator> separators, string input)
        {
            return separators
                .Where(separator => PreviousSeparatorSymbol(separator) != '\\')
                .ToList();

            char PreviousSeparatorSymbol(Separator separator) =>
                (separator.Index != 0) ? input[separator.Index - 1] : ' ';
        }

        static IEnumerable<Separator> RemoveAllUnclosedSeparators(this IEnumerable<Separator> separators)
        {
            var result = new List<Separator>();

            var stack = new Stack<Separator>();
            foreach (var separator in separators)
            {
                if (separator.Type == SeparatorType.Opening)
                    stack.Push(separator);
                if (separator.Type == SeparatorType.Closing && stack.Count != 0)
                {
                    var stackTopSeparator = stack.Peek();
                    if (stackTopSeparator.Tag == separator.Tag)
                    {
                        result.Add(stack.Pop());
                        result.Add(separator);
                    }
                }
            }

            return result.OrderBy(x => x.Index).ToList();
        }

        static IEnumerable<Separator> RemoveIncorrectNestingSeparators(this IEnumerable<Separator> separators)
        {
            foreach (var separator in separators) yield return separator;
        }
    }
}
