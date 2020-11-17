using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagLexer
    {
        public static List<TagInfoWithIndex> FindTagsIndexes(string sourceText, ITagInfo[] tagInfos)
        {
            var tagsWithIndexList = new List<TagInfoWithIndex>();
            foreach (var tagInfo in tagInfos)
            {
                var tagLength = tagInfo.OpenTagInMd.Length;
                var index = 0;
                do
                {
                    var substring = sourceText.Substring(index, tagLength);
                    if (tagInfo.OpenTagInMd == substring)
                        tagsWithIndexList.Add(new TagInfoWithIndex(tagInfo, index, substring));
                    else if (tagInfo is LinkTagInfo linkTagInfo &&
                             linkTagInfo.TagSymbols.Contains(sourceText[index]))
                        tagsWithIndexList.Add(new TagInfoWithIndex(tagInfo, index, sourceText[index].ToString()));
                } while (++index < sourceText.Length - tagLength + 1);
            }

            return FilterTags(sourceText, tagsWithIndexList);
        }

        private static List<TagInfoWithIndex> FilterTags(string sourceText, List<TagInfoWithIndex> tagsList)
        {
            var filteredList = new List<TagInfoWithIndex>();
            foreach (var tagInfoWithIndex in tagsList
                .OrderByDescending(tagInfoWithIndex => tagInfoWithIndex.TagInfo.OpenTagInMd.Length)
                .ThenBy(tagInfoWithIndex => tagInfoWithIndex.StartIndex))
            {
                if (IsScreened(sourceText, tagInfoWithIndex.StartIndex) || filteredList.Any(filteredTagAndIndex =>
                    tagInfoWithIndex.StartIndex >= filteredTagAndIndex.StartIndex &&
                    tagInfoWithIndex.StartIndex <= filteredTagAndIndex.StartIndex +
                    filteredTagAndIndex.TagInfo?.OpenTagInMd.Length - 1))
                    continue;

                filteredList.Add(tagInfoWithIndex);
            }

            FilterUnderlineTagsInsideWords(sourceText, filteredList);

            return filteredList;
        }

        private static void FilterUnderlineTagsInsideWords(string sourceText, List<TagInfoWithIndex> filteredList)
        {
            var underlineTagsInsideWords = FindUnderlineTagsInsideWords(sourceText, filteredList);
            var nonTags = GetTagsInsideWordsWithDigitHashset(sourceText, underlineTagsInsideWords);
            underlineTagsInsideWords.RemoveAll(tag => nonTags.Contains(tag));
            filteredList.RemoveAll(tag => nonTags.Contains(tag));
            nonTags = GetSeparatedTagsInsideWords(sourceText, filteredList, underlineTagsInsideWords);
            filteredList.RemoveAll(x => nonTags.Contains(x));
        }

        private static HashSet<TagInfoWithIndex> GetSeparatedTagsInsideWords(string sourceText,
            List<TagInfoWithIndex> filteredList, List<TagInfoWithIndex> underlineTagsInsideWords)
        {
            var nonTags = new HashSet<TagInfoWithIndex>();
            var underlineTagPairs = FindUnderlineTagsPairs(filteredList);
            var nonTagPairs = underlineTagPairs
                .Where(tagPair =>
                    AreInDiffParagraphs(sourceText, tagPair) ||
                    AreInDiffWords(sourceText, underlineTagsInsideWords, tagPair));
            foreach (var (firstTag, secondTag) in nonTagPairs)
            {
                nonTags.Add(firstTag);
                nonTags.Add(secondTag);
            }

            return nonTags;
        }

        private static bool AreInDiffWords(string sourceText, List<TagInfoWithIndex> underlineTagsInsideWords,
            KeyValuePair<TagInfoWithIndex, TagInfoWithIndex> tagPair)
        {
            return underlineTagsInsideWords.Contains(tagPair.Key) &&
                   underlineTagsInsideWords.Contains(tagPair.Value) && sourceText.IndexOf(" ", tagPair.Key.StartIndex,
                       tagPair.Value.StartIndex - tagPair.Key.StartIndex,
                       StringComparison.Ordinal) != -1;
        }

        private static bool AreInDiffParagraphs(string sourceText,
            KeyValuePair<TagInfoWithIndex, TagInfoWithIndex> tagPair)
        {
            return sourceText.IndexOf('\n', tagPair.Key.StartIndex,
                       tagPair.Value.StartIndex - tagPair.Key.StartIndex) !=
                   -1;
        }

        private static HashSet<TagInfoWithIndex> GetTagsInsideWordsWithDigitHashset(string sourceText,
            List<TagInfoWithIndex> underlineTagsInsideWords)
        {
            var nonTags = new HashSet<TagInfoWithIndex>();
            foreach (var tagAndIndex in underlineTagsInsideWords)
            {
                var index = tagAndIndex.StartIndex;
                var isNonTag = IsNonTag(sourceText, index, -1, i => i >= 0);

                if (isNonTag)
                {
                    nonTags.Add(tagAndIndex);
                    continue;
                }

                index = tagAndIndex.StartIndex + tagAndIndex.TagInfo.OpenTagInMd.Length - 1;
                isNonTag = IsNonTag(sourceText, index, 1, i => i < sourceText.Length);
                if (isNonTag)
                    nonTags.Add(tagAndIndex);
            }

            return nonTags;
        }

        private static Dictionary<TagInfoWithIndex, TagInfoWithIndex> FindUnderlineTagsPairs(
            List<TagInfoWithIndex> filteredList)
        {
            var underlineTagPairs = new Dictionary<TagInfoWithIndex, TagInfoWithIndex>();
            var tagStack = new Stack<TagInfoWithIndex>();
            foreach (var tagAndIndex in filteredList.Where(x =>
                x.TagInfo.OpenTagInMd == "_" || x.TagInfo.OpenTagInMd == "__"))
            {
                if (tagStack.Count == 0)
                {
                    tagStack.Push(tagAndIndex);
                    continue;
                }

                if (tagAndIndex.TagInfo.OpenTagInMd == tagStack.Peek().TagInfo.OpenTagInMd)
                {
                    underlineTagPairs.Add(tagStack.Pop(), tagAndIndex);
                    continue;
                }

                tagStack.Push(tagAndIndex);
            }

            return underlineTagPairs;
        }

        public static bool CanBeOpenTag(string sourceText, TagInfoWithIndex tagInfoWithIndex)
        {
            return tagInfoWithIndex.StartIndex + tagInfoWithIndex.TagInfo.OpenTagInMd.Length != sourceText.Length &&
                   !char.IsWhiteSpace(
                       sourceText[tagInfoWithIndex.StartIndex + tagInfoWithIndex.TagInfo.OpenTagInMd.Length]);
        }

        public static bool CanBeCloseTag(string sourceText, TagInfoWithIndex tagInfoWithIndex)
        {
            return char.IsWhiteSpace(sourceText[tagInfoWithIndex.StartIndex - 1]);
        }

        private static List<TagInfoWithIndex> FindUnderlineTagsInsideWords(string sourceText,
            List<TagInfoWithIndex> filteredList)
        {
            var tagsInWords = filteredList.Where(tagAndIndex =>
                (tagAndIndex.TagInfo.OpenTagInMd == "_" || tagAndIndex.TagInfo.OpenTagInMd == "__") &&
                tagAndIndex.StartIndex != 0
                && tagAndIndex.StartIndex + tagAndIndex.TagInfo.OpenTagInMd.Length != sourceText.Length
                && !char.IsWhiteSpace(sourceText[tagAndIndex.StartIndex - 1]) &&
                !char.IsWhiteSpace(sourceText[tagAndIndex.StartIndex + tagAndIndex.TagInfo.OpenTagInMd.Length]));
            return tagsInWords.ToList();
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