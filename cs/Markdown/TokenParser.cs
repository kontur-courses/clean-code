using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TokenParser
    {
        public static Token[] ParseStringToMdTokens(string sourceText, params TagInfo[] tagInfos)
        {
            var tagsList = FindTagsIndexes(sourceText, tagInfos);
            tagsList = TokenFilter.FilterTags(tagsList, sourceText);
            var tokens = FindTokens(sourceText, tagsList);
            TokenFilter.FilterIntersections(tokens);
            TokenFilter.FilterEmptyTokens(tokens);

            return tokens.ToArray();
        }

        public static List<Token> FindTokens(string sourceText, List<TagInfoWithIndex> tagsList)
        {
            var tagsStack = new Stack<TagInfoWithIndex>();
            var tokens = new List<Token>();
            FillTagsStack(sourceText, tagsList, tokens, tagsStack);

            return tokens;
        }

        private static void FillTagsStack(string sourceText, List<TagInfoWithIndex> tagsList, List<Token> tokens,
            Stack<TagInfoWithIndex> tagsStack)
        {
            foreach (var currentTagAndIndex in tagsList.OrderBy(tagAndIndex => tagAndIndex.StartIndex))
            {
                var currentTagInfo = currentTagAndIndex.TagInfo;

                if (currentTagInfo.IsSingle)
                {
                    AddTokenWithSingleTag(sourceText, currentTagInfo, currentTagAndIndex, tokens);
                    continue;
                }

                if (tagsStack.Count == 0)
                {
                    if (TokenFilter.CanBeOpenTag(sourceText, currentTagAndIndex))
                        tagsStack.Push(currentTagAndIndex);
                    continue;
                }

                var currentTagInMd = currentTagInfo.TagInMd;
                var peekTagInMd = tagsStack.Peek().TagInfo.TagInMd;
                if (currentTagInMd == peekTagInMd)
                {
                    if (TokenFilter.CanBeCloseTag(sourceText, currentTagAndIndex))
                        continue;
                    AddTokenWithDoubleTag(currentTagAndIndex, currentTagInMd, tagsStack, tokens, currentTagInfo);
                    continue;
                }

                if (TokenFilter.CanBeOpenTag(sourceText, currentTagAndIndex))
                    tagsStack.Push(new TagInfoWithIndex(currentTagInfo, currentTagAndIndex.StartIndex));
            }
        }

        private static void AddTokenWithDoubleTag(TagInfoWithIndex currentTagAndIndex, string currentTagInMd,
            Stack<TagInfoWithIndex> tagsStack,
            List<Token> tokens, TagInfo currentTagInfo)
        {
            var length = currentTagAndIndex.StartIndex + currentTagInMd.Length - tagsStack.Peek().StartIndex;
            tokens.Add(new Token(tagsStack.Peek().StartIndex,
                length,
                currentTagInfo));
            tagsStack.Pop();
        }

        private static void AddTokenWithSingleTag(string sourceText, TagInfo currentTagInfo,
            TagInfoWithIndex currentTagAndIndex, List<Token> tokens)
        {
            var tagEndIndex = sourceText.IndexOf(currentTagInfo.TagEndSym, currentTagAndIndex.StartIndex,
                StringComparison.Ordinal);
            var length = tagEndIndex != -1
                ? tagEndIndex - currentTagAndIndex.StartIndex
                : sourceText.Length - currentTagAndIndex.StartIndex;
            tokens.Add(new Token(currentTagAndIndex.StartIndex, length, currentTagAndIndex.TagInfo));
        }

        private static List<TagInfoWithIndex> FindTagsIndexes(string sourceText, TagInfo[] tagInfos)
        {
            var tagsWithIndexList = new List<TagInfoWithIndex>();
            var sortedTagInfos = tagInfos
                .OrderByDescending(tag => tag.TagForConverting.Length);
            foreach (var tagInfo in sortedTagInfos)
            {
                var tagLength = tagInfo.TagInMd.Length;
                var index = 0;
                do
                {
                    var substring = sourceText.Substring(index, tagLength);
                    if (tagInfo.TagInMd != substring)
                        continue;
                    tagsWithIndexList.Add(new TagInfoWithIndex(tagInfo, index));
                } while (++index < sourceText.Length - tagLength + 1);
            }

            return tagsWithIndexList;
        }

        public static string ScreenSymbols(string sourceText, params TagInfo[] tagInfos)
        {
            if (sourceText.Length == 0)
                return sourceText;
            var resultStr = new StringBuilder();
            var index = 0;
            do
            {
                if (index + 1 < sourceText.Length && sourceText[index] == '\\' &&
                    (tagInfos.Any(tagInfo => sourceText[index + 1] == tagInfo.TagInMd[0]) ||
                     sourceText[index + 1] == '\\'))
                    index++;

                resultStr.Append(sourceText[index]);
            } while (++index < sourceText.Length);

            return resultStr.ToString();
        }
    }
}