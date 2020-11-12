using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly int StartIndex;
        public readonly TagInfo TagInfo;

        public Token(int startIndex, int length, TagInfo tagInfo)
        {
            StartIndex = startIndex;
            Length = length;
            TagInfo = tagInfo;
        }

        public static Token[] ParseStringToMdTokens(string sourceText, params TagInfo[] tagInfos)
        {
            var tokens = new List<Token>();
            var tagsList = FindTagsIndexes(sourceText, tagInfos);
            var tagsStack = new Stack<(TagInfo tagInfo, int startIndex)>();
            foreach (var tagAndIndex in tagsList.OrderBy(tagAndIndex => tagAndIndex.startIndex))
            {
                if (tagsStack.Count == 0)
                {
                    tagsStack.Push(tagAndIndex);
                    continue;
                }

                var tagInfo = tagAndIndex.tagInfo;
                var tagInMd = tagInfo.TagInMd;
                var peekTag = tagsStack.Peek().tagInfo.TagInMd;
                if (tagInMd == "__" && peekTag == "_")
                    continue;
                if (tagInMd == peekTag)
                {
                    var length = tagAndIndex.startIndex + tagInMd.Length - tagsStack.Peek().startIndex;
                    tokens.Add(new Token(tagsStack.Peek().startIndex,
                        length,
                        tagInfo));
                    tagsStack.Pop();
                    continue;
                }

                tagsStack.Push((tagInfo, tagAndIndex.startIndex));
            }

            return tokens.ToArray();
        }

        private static List<(TagInfo tagInfo, int startIndex)> FindTagsIndexes(string sourceText, TagInfo[] tagInfos)
        {
            var tagsList = new List<(TagInfo tagInfo, int startIndex)>();
            var groupedTagInfos = tagInfos
                .GroupBy(tag => tag.OpenTag.Length);
            foreach (var group in groupedTagInfos)
            {
                var tagLength = group.First().TagInMd.Length;
                var index = 0;
                while (index < sourceText.Length - tagLength + 1)
                {
                    var substring = sourceText.Substring(index, tagLength);
                    foreach (var tagInfo in group)
                    {
                        if (tagInfo.TagInMd != substring || tagsList.Any(tagAndIndex =>
                            index >= tagAndIndex.startIndex &&
                            index <= tagAndIndex.startIndex + tagAndIndex.tagInfo?.TagInMd.Length))
                            continue;
                        tagsList.Add((tagInfo, index));
                    }

                    index++;
                }
            }

            return tagsList;
        }

        public static string ApplyTokensToString(string sourceText, Token[] tokens)
        {
            var influence = 0;
            foreach (var token in tokens.OrderByDescending(token => token.StartIndex))
            {
                var tagInMd = token.TagInfo.TagInMd.Length;
                sourceText = sourceText.Replace(token.TagInfo.OpenTag, token.StartIndex + influence, tagInMd);
                influence += token.TagInfo.OpenTag.Length - tagInMd;
                sourceText = sourceText.Replace(token.TagInfo.CloseTag, token.StartIndex + token.Length + influence - 1,
                    tagInMd);
                influence += token.TagInfo.CloseTag.Length - tagInMd;
            }

            return sourceText;
        }
    }
}