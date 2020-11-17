using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TokenParser
    {
        public static IToken[] ParseStringToMdTokens(string sourceText, params ITagInfo[] tagInfos)
        {
            var tagsList = TagLexer.FindTagsIndexes(sourceText, tagInfos);
            var tokens = FindTokens(sourceText, tagsList);

            return tokens.ToArray();
        }

        private static List<IToken> FindTokens(string sourceText, List<TagInfoWithIndex> tagsList)
        {
            var tokens = GetTokensList(sourceText, tagsList);
            TokenLexer.FilterTokens(tokens);

            return tokens;
        }

        private static List<IToken> GetTokensList(string sourceText, List<TagInfoWithIndex> tagsList)
        {
            var tokens = new List<IToken>();
            AddAttributeTokens(sourceText, tagsList, tokens);
            AddEmphasizingTokens(sourceText, tagsList, tokens);
            AddTokensWithSingleTags(sourceText, tagsList, tokens);

            return tokens;
        }

        private static void AddEmphasizingTokens(string sourceText, List<TagInfoWithIndex> tagsList,
            List<IToken> tokens)
        {
            var tagsStack = new Stack<TagInfoWithIndex>();
            foreach (var currentTagAndIndex in tagsList.Where(tagAndIndex =>
                tagAndIndex.TagInfo is EmphasizingTagInfo && !tokens.Any(token =>
                    token.TagInfo is IAttributeTagInfo && tagAndIndex.StartIndex > token.StartIndex &&
                    tagAndIndex.StartIndex < token.StartIndex + token.Length - 1)))
            {
                var currentTagInfo = currentTagAndIndex.TagInfo;

                if (tagsStack.Count == 0 || currentTagInfo.OpenTagInMd != tagsStack.Peek().TagInfo.OpenTagInMd)
                {
                    if (TagLexer.CanBeOpenTag(sourceText, currentTagAndIndex))
                        tagsStack.Push(currentTagAndIndex);
                    continue;
                }

                if (TagLexer.CanBeCloseTag(sourceText, currentTagAndIndex))
                    continue;

                var length = currentTagAndIndex.StartIndex + currentTagInfo.OpenTagInMd.Length -
                             tagsStack.Peek().StartIndex;
                tokens.Add(new EmphasizingToken(tagsStack.Peek().StartIndex,
                    length,
                    currentTagInfo));
                tagsStack.Pop();
            }
        }

        private static void AddTokensWithSingleTags(string sourceText, List<TagInfoWithIndex> tagsList,
            List<IToken> tokens)
        {
            foreach (var currentTagAndIndex in tagsList.Where(tagAndIndex => tagAndIndex.TagInfo is SingleTagInfo))
            {
                var singleTagInfo = currentTagAndIndex.TagInfo as SingleTagInfo;
                var tagEndIndex = singleTagInfo?.TagEndSym != default
                    ? sourceText.IndexOf(singleTagInfo.TagEndSym, currentTagAndIndex.StartIndex,
                        StringComparison.Ordinal)
                    : -1;
                var length = tagEndIndex != -1
                    ? tagEndIndex - currentTagAndIndex.StartIndex
                    : sourceText.Length - currentTagAndIndex.StartIndex;
                tokens.Add(new TokenWithSingleTag(currentTagAndIndex.StartIndex, length, currentTagAndIndex.TagInfo));
            }
        }

        private static void AddAttributeTokens(string sourceText, List<TagInfoWithIndex> tagsList, List<IToken> tokens)
        {
            var tagsStack = new Stack<TagInfoWithIndex>();
            foreach (var currentTagAndIndex in tagsList.Where(tagAndIndex => tagAndIndex.TagInfo is IAttributeTagInfo))
                if (currentTagAndIndex.TagSubstring == currentTagAndIndex.TagInfo.OpenTagInMd && tagsStack.Count == 0
                    || tagsStack.Count != 0 && (currentTagAndIndex.TagSubstring == "]" &&
                                                tagsStack.Peek().TagSubstring == currentTagAndIndex.TagInfo.OpenTagInMd
                                                || currentTagAndIndex.TagSubstring == "(" &&
                                                tagsStack.Peek().TagSubstring == "]" &&
                                                currentTagAndIndex.StartIndex == tagsStack.Peek().StartIndex + 1))
                {
                    tagsStack.Push(currentTagAndIndex);
                }
                else
                {
                    if (tagsStack.Count == 0 || tagsStack.Peek().TagSubstring != "(") continue;
                    var attributeToken = CreateAttributeToken(sourceText, currentTagAndIndex, tagsStack);
                    tokens.Add(attributeToken);
                }
        }

        private static AttributeToken CreateAttributeToken(string sourceText, TagInfoWithIndex currentTagAndIndex,
            Stack<TagInfoWithIndex> tagsStack)
        {
            var attributeEndIndex = currentTagAndIndex.StartIndex - 1;
            var attributeStartIndex = tagsStack.Pop().StartIndex + 1;
            var titleEndIndex = tagsStack.Pop().StartIndex - 1;
            var titleStart = tagsStack.Pop();
            var titleStartIndex = titleStart.StartIndex + titleStart.TagSubstring.Length;
            var length = attributeEndIndex - titleStart.StartIndex + 1;
            var title = sourceText.Substring(titleStartIndex, titleEndIndex - titleStartIndex + 1);
            var attribute = sourceText.Substring(attributeStartIndex,
                attributeEndIndex - attributeStartIndex + 1);
            var attributeToken = new AttributeToken(titleStart.StartIndex, length, titleStart.TagInfo,
                title, attribute);
            return attributeToken;
        }

        public static string ScreenSymbols(string sourceText, params ITagInfo[] tagInfos)
        {
            if (sourceText.Length == 0)
                return sourceText;
            var resultStr = new StringBuilder();
            var index = 0;
            do
            {
                if (index + 1 < sourceText.Length && sourceText[index] == '\\' &&
                    (tagInfos.Any(tagInfo => sourceText[index + 1] == tagInfo.OpenTagInMd[0]) ||
                     sourceText[index + 1] == '\\'))
                    index++;

                resultStr.Append(sourceText[index]);
            } while (++index < sourceText.Length);

            return resultStr.ToString();
        }
    }
}