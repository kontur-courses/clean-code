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
            tagsList = FilterTags(tagsList, sourceText);
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

        private static List<TagInfoWithIndex> FilterTags(List<TagInfoWithIndex> tagsList, string sourceText)
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

            return filteredList;
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