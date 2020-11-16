using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenFilter
    {
        public static void FilterIntersections(List<Token> tokens)
        {
            var nonTokens = tokens.Where(token => tokens.Any(x =>
                token != x && !AreNested(token, x) && token.StartIndex < x.StartIndex + x.Length &&
                x.StartIndex < token.StartIndex + token.Length)).ToHashSet();
            tokens.RemoveAll(token => nonTokens.Contains(token));
        }

        public static void FilterEmptyTokens(List<Token> tokens)
        {
            var nonTokens = tokens.Where(token => token.Length == token.TagInfo.TagInMd.Length * 2).ToHashSet();
            tokens.RemoveAll(token => nonTokens.Contains(token));
        }

        private static bool AreNested(Token firstToken, Token secondToken)
        {
            return firstToken.StartIndex > secondToken.StartIndex && firstToken.EndTagIndex < secondToken.EndTagIndex
                   || secondToken.StartIndex > firstToken.StartIndex &&
                   secondToken.EndTagIndex < firstToken.EndTagIndex;
        }

        public static List<TagInfoWithIndex> FilterTags(List<TagInfoWithIndex> tagsList, string sourceText)
        {
            var filteredList = new List<TagInfoWithIndex>();
            foreach (var tagInfoWithIndex in tagsList
                .OrderByDescending(tagInfoWithIndex => tagInfoWithIndex.TagInfo.TagInMd.Length)
                .ThenBy(tagInfoWithIndex => tagInfoWithIndex.StartIndex))
            {
                if (IsScreened(sourceText, tagInfoWithIndex.StartIndex) || filteredList.Any(filteredTagAndIndex =>
                    tagInfoWithIndex.StartIndex >= filteredTagAndIndex.StartIndex &&
                    tagInfoWithIndex.StartIndex <= filteredTagAndIndex.StartIndex +
                    filteredTagAndIndex.TagInfo?.TagInMd.Length - 1))
                    continue;

                filteredList.Add(tagInfoWithIndex);
            }

            FilterDoubleUnderlineInSingle(sourceText, filteredList);
            FilterTagsInWordsWithDigits(sourceText, filteredList);
            FilterTagsInDifferentWords(sourceText, filteredList);
            FilterTagsInDiffParagraphs(sourceText, filteredList);

            return filteredList;
        }

        private static void FilterTagsInDiffParagraphs(string sourceText, List<TagInfoWithIndex> filteredList)
        {
            var underlineTagPairs = FindUnderlineTagsPairs(filteredList);
            var nonTagPairs = underlineTagPairs.Where(tagPair =>
                sourceText.IndexOf('\n', tagPair.Key.StartIndex, tagPair.Value.StartIndex - tagPair.Key.StartIndex) !=
                -1).ToHashSet();
            var nonTags = new HashSet<TagInfoWithIndex>();
            foreach (var (firstTag, secondTag) in nonTagPairs)
            {
                nonTags.Add(firstTag);
                nonTags.Add(secondTag);
            }

            filteredList.RemoveAll(x => nonTags.Contains(x));
        }

        private static void FilterTagsInDifferentWords(string sourceText, List<TagInfoWithIndex> filteredList)
        {
            var underlineTagPairs = FindUnderlineTagsPairs(filteredList);
            var underlineTagsInsideWords = FindUnderlineTagsInsideWords(sourceText, filteredList).ToHashSet();
            var nonTagPairs = underlineTagPairs
                .Where(tagPair => underlineTagsInsideWords.Contains(tagPair.Key) &&
                                  underlineTagsInsideWords.Contains(tagPair.Value))
                .Where(tagPair =>
                    sourceText.IndexOf(" ", tagPair.Key.StartIndex, tagPair.Value.StartIndex - tagPair.Key.StartIndex,
                        StringComparison.Ordinal) != -1);
            var nonTags = new HashSet<TagInfoWithIndex>();
            foreach (var (firstTag, secondTag) in nonTagPairs)
            {
                nonTags.Add(firstTag);
                nonTags.Add(secondTag);
            }

            filteredList.RemoveAll(x => nonTags.Contains(x));
        }

        private static Dictionary<TagInfoWithIndex, TagInfoWithIndex> FindUnderlineTagsPairs(
            List<TagInfoWithIndex> filteredList)
        {
            var underlineTagPairs = new Dictionary<TagInfoWithIndex, TagInfoWithIndex>();
            var tagStack = new Stack<TagInfoWithIndex>();
            foreach (var tagAndIndex in filteredList.Where(x => x.TagInfo.TagInMd == "_" || x.TagInfo.TagInMd == "__"))
            {
                if (tagStack.Count == 0)
                {
                    tagStack.Push(tagAndIndex);
                    continue;
                }

                if (tagAndIndex.TagInfo.TagInMd == tagStack.Peek().TagInfo.TagInMd)
                {
                    underlineTagPairs.Add(tagStack.Pop(), tagAndIndex);
                    continue;
                }

                tagStack.Push(tagAndIndex);
            }

            return underlineTagPairs;
        }

        private static void FilterTagsInWordsWithDigits(string sourceText, List<TagInfoWithIndex> filteredList)
        {
            var tagsInWords = FindUnderlineTagsInsideWords(sourceText, filteredList);
            var nonTags = new HashSet<TagInfoWithIndex>();
            foreach (var tagAndIndex in tagsInWords)
            {
                var index = tagAndIndex.StartIndex;
                var isNonTag = IsNonTag(sourceText, index, -1, i => i >= 0);

                if (isNonTag)
                {
                    nonTags.Add(tagAndIndex);
                    continue;
                }

                index = tagAndIndex.StartIndex + tagAndIndex.TagInfo.TagInMd.Length - 1;
                isNonTag = IsNonTag(sourceText, index, 1, i => i < sourceText.Length);
                if (isNonTag)
                    nonTags.Add(tagAndIndex);
            }

            filteredList.RemoveAll(x => nonTags.Contains(x));
        }

        public static bool CanBeOpenTag(string sourceText, TagInfoWithIndex tagInfoWithIndex)
        {
            return tagInfoWithIndex.TagInfo.TagInMd != "_" && tagInfoWithIndex.TagInfo.TagInMd != "__"
                   || tagInfoWithIndex.StartIndex + tagInfoWithIndex.TagInfo.TagInMd.Length != sourceText.Length &&
                   !char.IsWhiteSpace(
                       sourceText[tagInfoWithIndex.StartIndex + tagInfoWithIndex.TagInfo.TagInMd.Length]);
        }

        public static bool CanBeCloseTag(string sourceText, TagInfoWithIndex tagInfoWithIndex)
        {
            return char.IsWhiteSpace(sourceText[tagInfoWithIndex.StartIndex - 1]);
        }

        private static IEnumerable<TagInfoWithIndex> FindUnderlineTagsInsideWords(string sourceText,
            List<TagInfoWithIndex> filteredList)
        {
            var tagsInWords = filteredList.Where(tagAndIndex =>
                (tagAndIndex.TagInfo.TagInMd == "_" || tagAndIndex.TagInfo.TagInMd == "__") &&
                tagAndIndex.StartIndex != 0
                && tagAndIndex.StartIndex + tagAndIndex.TagInfo.TagInMd.Length != sourceText.Length
                && !char.IsWhiteSpace(sourceText[tagAndIndex.StartIndex - 1]) &&
                !char.IsWhiteSpace(sourceText[tagAndIndex.StartIndex + tagAndIndex.TagInfo.TagInMd.Length]));
            return tagsInWords;
        }

        private static bool IsNonTag(string sourceText, int index, int iterator, Func<int, bool> checkBorder)
        {
            var isNonTag = false;
            while (checkBorder(index + iterator) && !char.IsWhiteSpace(sourceText[index + iterator]))
            {
                index += iterator;
                if (!char.IsDigit(sourceText[index])) continue;
                isNonTag = true;
                break;
            }

            return isNonTag;
        }

        private static void FilterDoubleUnderlineInSingle(string sourceText,
            List<TagInfoWithIndex> filteredList)
        {
            var tokens = TokenParser.FindTokens(sourceText, filteredList);
            var tokensWithOnlyItalicTag = tokens.Where(token => token.TagInfo.TagInMd == "_");
            var nonTags = filteredList
                .Where(tagInfoWithIndex => tagInfoWithIndex.TagInfo.TagInMd == "__" && tokensWithOnlyItalicTag.Any(
                    token =>
                        tagInfoWithIndex.StartIndex >= token.StartIndex &&
                        tagInfoWithIndex.StartIndex < token.StartIndex + token.Length))
                .ToHashSet();
            filteredList.RemoveAll(x => nonTags.Contains(x));
        }

        private static bool IsScreened(string sourceText, int tagStartIndex)
        {
            if (tagStartIndex == 0)
                return false;
            var count = 0;
            var index = tagStartIndex;
            while (index > 0 && sourceText[--index] == '\\')
                count++;
            return count % 2 == 1;
        }
    }
}