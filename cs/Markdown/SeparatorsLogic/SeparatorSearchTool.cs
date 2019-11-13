using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    static class SeparatorSearchTool
    {
        static HashSet<char> SpecialSymbols;

        public static Dictionary<string, List<Separator>> GetSeparators(string input, List<string> separatorsTags)
        {
            if (input == null) throw new ArgumentNullException();
            if (separatorsTags.Count == 0) return new Dictionary<string, List<Separator>>();

            SpecialSymbols = separatorsTags.SelectMany(x => x).ToHashSet();

            Dictionary<string, List<int>> IndicesOfSeparators = GetIndicesOfSeparators(input, separatorsTags);
            Dictionary<string, List<Separator>> convertedSeparators = ConvertSeparators(input, IndicesOfSeparators);

            return convertedSeparators;
        }

        static Dictionary<string, List<Separator>> ConvertSeparators(string input, Dictionary<string, List<int>> indexesOfSeparators)
        {
            var convertedSeparators = new Dictionary<string, List<Separator>>();

            foreach (string separatorsTags in indexesOfSeparators.Keys)
                foreach (var index in indexesOfSeparators[separatorsTags])
                {
                    Separator sep;
                    if (TryGetSeparator(input, index, separatorsTags, out sep))
                        if (!convertedSeparators.ContainsKey(separatorsTags))
                            convertedSeparators[separatorsTags] = new List<Separator> { sep };
                        else
                            convertedSeparators[separatorsTags].Add(sep);
                }

            return convertedSeparators;
        }

        static Dictionary<string, List<int>> GetIndicesOfSeparators(string input, List<string> separators)
        {
            var result = new Dictionary<string, List<int>>();

            foreach(var separator in separators)
                result.Add(separator, GetSubstringIndices(input, separator));

            return result;
        }

        static List<int> GetSubstringIndices(string source, string substring)
        {
            var indices = new List<int>();

            int index = source.IndexOf(substring, 0);
            while (index > -1)
            {
                indices.Add(index);
                index = source.IndexOf(substring, index + substring.Length);
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
            && !SpecialSymbols.Contains(previousSeparatorSymbol) 
            && !SpecialSymbols.Contains(nextSeparatorSymbol);

        static bool IsClosingSeparator(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !SpecialSymbols.Contains(nextSeparatorSymbol)
            && !SpecialSymbols.Contains(previousSeparatorSymbol);
    }
}
