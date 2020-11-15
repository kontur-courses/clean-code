using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenParser
    {
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
                .GroupBy(tag => tag.TagForConverting.Length);
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
    }
}