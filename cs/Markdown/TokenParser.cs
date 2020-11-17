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
            var tagsList = TagLexer.FindTagsIndexes(sourceText, tagInfos);
            var tokens = FindTokens(sourceText, tagsList);

            return tokens.ToArray();
        }

        public static List<Token> FindTokens(string sourceText, List<TagInfoWithIndex> tagsList)
        {
            var tagsStack = new Stack<TagInfoWithIndex>();
            var tokens = new List<Token>();
            FillTagsStack(sourceText, tagsList, tokens, tagsStack);
            TokenLexer.FilterTokens(tokens);

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
                    if (TagLexer.CanBeOpenTag(sourceText, currentTagAndIndex))
                        tagsStack.Push(currentTagAndIndex);
                    continue;
                }

                var currentTagInMd = currentTagInfo.TagInMd;
                var peekTagInMd = tagsStack.Peek().TagInfo.TagInMd;
                if (currentTagInMd == peekTagInMd)
                {
                    if (TagLexer.CanBeCloseTag(sourceText, currentTagAndIndex))
                        continue;
                    AddTokenWithDoubleTag(currentTagAndIndex, currentTagInMd, tagsStack, tokens, currentTagInfo);
                    continue;
                }

                if (TagLexer.CanBeOpenTag(sourceText, currentTagAndIndex))
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