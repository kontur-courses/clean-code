using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class SeparatorSearchTool
    {
        static HashSet<char> _specialSymbols;

        public static Dictionary<string, List<Separator>> GetSeparators(string input, List<string> separatorsTags)
        {
            if (input == null) throw new ArgumentNullException();
            if (separatorsTags.Count == 0) return new Dictionary<string, List<Separator>>();

            _specialSymbols = separatorsTags.SelectMany(x => x).ToHashSet();

            var indicesOfSeparators = GetIndicesOfSeparators(input, separatorsTags);
            var convertedSeparators = ConvertSeparators(input, indicesOfSeparators);

            return convertedSeparators;
        }

        static Dictionary<string, List<Separator>> ConvertSeparators(string input, Dictionary<string, List<int>> indexesOfSeparators)
        {
            var convertedSeparators = new Dictionary<string, List<Separator>>();

            foreach (var separatorTag in indexesOfSeparators.Keys)
                foreach (var index in indexesOfSeparators[separatorTag])
                {
                    if (!TryGetSeparator(input, index, separatorTag, out Separator separator)) continue;

                    if (!convertedSeparators.ContainsKey(separatorTag))
                        convertedSeparators[separatorTag] = new List<Separator> { separator };
                    else
                        convertedSeparators[separatorTag].Add(separator);
                }

            return convertedSeparators;
        }

        static Dictionary<string, List<int>> GetIndicesOfSeparators(string input, List<string> separators)
        {
            return separators.ToDictionary(separator => separator, separator => GetSubstringIndices(input, separator));
        }

        static List<int> GetSubstringIndices(string source, string substring)
        {
            var indices = new List<int>();

            var index = source
                .IndexOf(substring, 0, StringComparison.Ordinal);
            while (index > -1)
            {
                indices.Add(index);
                index = source
                    .IndexOf(substring, index + substring.Length, StringComparison.Ordinal);
            }

            return indices;
        }

        static bool TryGetSeparator(string input, int separatorIndex, string separatorToConvert, out Separator separator)
        {
            var previousSeparatorSymbol = (separatorIndex == 0) ? 
                ' ' : 
                input[separatorIndex - 1];
            var nextSeparatorSymbol = (separatorIndex + separatorToConvert.Length >= input.Length) ? 
                ' ' : 
                input[separatorIndex + separatorToConvert.Length];

            if (IsOpeningSeparator(previousSeparatorSymbol, nextSeparatorSymbol))
            {
                separator = new Separator(separatorToConvert, separatorIndex, SeparatorType.Opening);
                return true;
            }

            if (IsClosingSeparator(previousSeparatorSymbol, nextSeparatorSymbol))
            {
                separator = new Separator(separatorToConvert, separatorIndex, SeparatorType.Closing);
                return true;
            }

            separator = null;
            return false;
        }

        static bool IsOpeningSeparator(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(previousSeparatorSymbol)
            && !char.IsWhiteSpace(nextSeparatorSymbol)
            && !_specialSymbols.Contains(previousSeparatorSymbol) 
            && !_specialSymbols.Contains(nextSeparatorSymbol);

        static bool IsClosingSeparator(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !_specialSymbols.Contains(nextSeparatorSymbol)
            && !_specialSymbols.Contains(previousSeparatorSymbol);
    }
}
