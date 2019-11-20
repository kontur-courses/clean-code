using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagParser
    {
        private static HashSet<char> specialSymbols;

        public static List<Tag> Parse(string inputString, List<string> tagDesignations)
        {
            specialSymbols = tagDesignations.SelectMany(x => x).ToHashSet();

            var indicesOfMarkdownTags = tagDesignations
                .ToDictionary(
                    tagDesignation => tagDesignation,
                    tagDesignation => GetTagIndices(inputString, tagDesignation));

            var convertedTags = ConvertToTagClass(inputString, indicesOfMarkdownTags);

            return convertedTags;
        }

        private static List<Tag> ConvertToTagClass(string inputString, Dictionary<string, List<int>> indexesOfTags)
        {
            var convertedTags = new List<Tag>();

            foreach (var tagDesignation in indexesOfTags.Keys)
            foreach (var index in indexesOfTags[tagDesignation])
                if (TryGetTag(inputString, index, tagDesignation, out Tag tag))
                    convertedTags.Add(tag);

            return convertedTags;
        }

        private static List<int> GetTagIndices(string input, string tagDesignations)
        {
            return input.GetSubstringIndices(tagDesignations);
        }

        private static bool TryGetTag(string inputString, int tagIndex, string tagDesignation, out Tag tag)
        {
            var previousTagSymbol = (tagIndex != 0) 
                ? inputString[tagIndex - 1]
                : ' ';

            var nextTagSymbol = (tagIndex + tagDesignation.Length < inputString.Length)
                ? inputString[tagIndex + tagDesignation.Length]
                : ' ';

            if (IsOpeningTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(
                    tagDesignation, 
                    tagIndex, 
                    TagType.Opening,
                    Markdown.TagsInfo[tagDesignation].Priority);
                return true;
            }

            if (IsClosingTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(
                    tagDesignation,
                    tagIndex,
                    TagType.Closing,
                    Markdown.TagsInfo[tagDesignation].Priority);
                return true;
            }

            tag = null;
            return false;
        }

        private static bool IsOpeningTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(previousSeparatorSymbol)
            && !char.IsWhiteSpace(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol) 
            && !specialSymbols.Contains(nextSeparatorSymbol);

        private static bool IsClosingTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !specialSymbols.Contains(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol);
    }
}
