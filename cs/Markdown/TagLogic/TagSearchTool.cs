using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagSearchTool
    {
        static HashSet<char> specialSymbols;

        public static Dictionary<string, List<Tag>> GetMarkdownTags(string inputString, List<string> tagSymbols)
        {
            if (inputString == null) throw new ArgumentNullException();
            if (tagSymbols.Count == 0) return new Dictionary<string, List<Tag>>();

            specialSymbols = tagSymbols.SelectMany(x => x).ToHashSet();

            var indicesOfMarkdownTags = GetIndicesOfTags(inputString, tagSymbols);
            var convertedTags = ConvertToTagClass(inputString, indicesOfMarkdownTags);

            return convertedTags;
        }

        static Dictionary<string, List<Tag>> ConvertToTagClass(string inputString, Dictionary<string, List<int>> indexesOfTags)
        {
            var convertedTags = new Dictionary<string, List<Tag>>();

            foreach (var separatorTagSymbol in indexesOfTags.Keys)
            foreach (var index in indexesOfTags[separatorTagSymbol])
            {
                if (!TryGetTag(inputString, index, separatorTagSymbol, out Tag separator)) continue;

                if (!convertedTags.ContainsKey(separatorTagSymbol))
                    convertedTags[separatorTagSymbol] = new List<Tag> {separator};
                else
                    convertedTags[separatorTagSymbol].Add(separator);
            }

            return convertedTags;
        }

        static Dictionary<string, List<int>> GetIndicesOfTags(string input, List<string> tagSymbols)
        {
            return tagSymbols
                .ToDictionary(separator => separator, separator => GetSubstringIndices(input, separator));
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

        static bool TryGetTag(string inputString, int tagIndex, string tagSymbolToConvert, out Tag tag)
        {
            var previousTagSymbol = (tagIndex == 0) ? 
                ' ' : 
                inputString[tagIndex - 1];
            var nextTagSymbol = (tagIndex + tagSymbolToConvert.Length >= inputString.Length) ? 
                ' ' : 
                inputString[tagIndex + tagSymbolToConvert.Length];

            if (IsOpeningTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(
                    tagSymbolToConvert, 
                    tagIndex, 
                    TagType.Opening, 
                    Markdown.MarkDownTagsPriority[tagSymbolToConvert]);
                return true;
            }

            if (IsClosingTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(
                    tagSymbolToConvert,
                    tagIndex,
                    TagType.Closing,
                    Markdown.MarkDownTagsPriority[tagSymbolToConvert]);
                return true;
            }

            tag = null;
            return false;
        }

        static bool IsOpeningTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(previousSeparatorSymbol)
            && !char.IsWhiteSpace(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol) 
            && !specialSymbols.Contains(nextSeparatorSymbol);

        static bool IsClosingTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !specialSymbols.Contains(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol);
    }
}
