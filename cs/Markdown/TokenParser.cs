using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TokenParser
    {
        public static Token[] ParseStringToMdTokens(string sourceText, params TagInfo[] tagInfos)
        {
            var tokens = new List<Token>();
            var tagsList = FindTagsIndexes(sourceText, tagInfos);
            var tagsStack = new Stack<TagInfoWithIndex>();
            foreach (var tagInfoWithIndex in tagsList.OrderBy(tagAndIndex => tagAndIndex.StartIndex))
            {
                if (tagsStack.Count == 0)
                {
                    tagsStack.Push(tagInfoWithIndex);
                    continue;
                }

                var tagInfo = tagInfoWithIndex.TagInfo;
                var tagInMd = tagInfo.TagInMd;
                var peekTag = tagsStack.Peek().TagInfo.TagInMd;
                if (tagInMd == "__" && peekTag == "_")
                    continue;
                if (tagInMd == peekTag)
                {
                    var length = tagInfoWithIndex.StartIndex + tagInMd.Length - tagsStack.Peek().StartIndex;
                    tokens.Add(new Token(tagsStack.Peek().StartIndex,
                        length,
                        tagInfo));
                    tagsStack.Pop();
                    continue;
                }

                tagsStack.Push(new TagInfoWithIndex(tagInfo, tagInfoWithIndex.StartIndex));
            }

            return tokens.ToArray();
        }

        private static List<TagInfoWithIndex> FindTagsIndexes(string sourceText, TagInfo[] tagInfos)
        {
            var tagsList = new List<TagInfoWithIndex>();
            var sortedTagInfos = tagInfos
                .OrderByDescending(tag => tag.TagForConverting.Length);
            foreach (var tagInfo in sortedTagInfos)
            {
                var tagLength = tagInfo.TagInMd.Length;
                var index = 0;
                do
                {
                    var substring = sourceText.Substring(index, tagLength);
                    if (tagInfo.TagInMd != substring || tagsList.Any(tagAndIndex =>
                        index >= tagAndIndex.StartIndex &&
                        index <= tagAndIndex.StartIndex + tagAndIndex.TagInfo?.TagInMd.Length))
                        continue;
                    tagsList.Add(new TagInfoWithIndex(tagInfo, index));
                } while (++index < sourceText.Length - tagLength + 1);
            }

            return tagsList;
        }
    }
}