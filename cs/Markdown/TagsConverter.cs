using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TagsConverter
    {
        public static string ConvertToNewSpecifications(StringBuilder sourceStrings, IReadOnlyCollection<TagSpecification> currentSpecifications, IReadOnlyCollection<TagSpecification> newSpecifications, IEnumerable<TagsPair> tagsPairs)
        {
            var sortedTags = GetTokensFromPairs(tagsPairs).Reverse();
            foreach (var token in sortedTags)
            {
                var newTagSpecification = newSpecifications.FirstOrDefault(t => t.TagType == token.TagType);
                if (newTagSpecification == null)
                {
                    throw new ArgumentException($"New specifications does not support tag type {token.TagType}");
                }
                var newToken = token.PositionType == PositionType.OpeningToken ? newTagSpecification.StartToken : newTagSpecification.EndToken;
                sourceStrings.Replace(token.Value, newToken, token.PositionInText, token.Value.Length);
            }
            return sourceStrings.ToString();
        }

        private static IEnumerable<Token> GetTokensFromPairs(IEnumerable<TagsPair> tagsPairs)
        {
            var sortedTags = new SortedList<int, Token>();
            foreach (var pair in tagsPairs)
            {
                sortedTags.Add(pair.StartPosition, pair.StartToken);
                sortedTags.Add(pair.EndPosition, pair.EndToken);
            }

            return sortedTags.Values;
        }
    }
}
