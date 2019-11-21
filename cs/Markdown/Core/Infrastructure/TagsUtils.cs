using System.Collections.Generic;
using System.Linq;

namespace Markdown.Core.Infrastructure
{
    public static class TagsUtils
    {
        public static readonly List<TagInfo> TagsInfo = new List<TagInfo>()
            {
                new TagInfo("__", "strong", LocationType.Inline),
                new TagInfo("_", "em", LocationType.Inline),
                new TagInfo("######", "h6", LocationType.Beginning),
                new TagInfo("#####", "h5", LocationType.Beginning),
                new TagInfo("####", "h4", LocationType.Beginning),
                new TagInfo("###", "h3", LocationType.Beginning),
                new TagInfo("##", "h2", LocationType.Beginning),
                new TagInfo("#", "h1", LocationType.Beginning),
            }
            .OrderByDescending(tagInfo => tagInfo.MdTag)
            .ToList();

        public static IEnumerable<TagInfo> InlineTagsInfo =>
            TagsInfo.Where(tagInfo => tagInfo.LocationType == LocationType.Inline);

        public static IEnumerable<TagInfo> BeginningTagsInfo =>
            TagsInfo.Where(tagInfo => tagInfo.LocationType == LocationType.Beginning);

        public static IEnumerable<string> MdInlineTags =>
            InlineTagsInfo.Select(tagInfo => tagInfo.MdTag);

        public static IEnumerable<string> MdBeginningTags =>
            BeginningTagsInfo.Select(tagInfo => tagInfo.MdTag);

        public static string GetTagNameByMdTag(string mdTag)
        {
            return TagsInfo.Find(tagInfo => tagInfo.MdTag == mdTag).TagName;
        }

        public static TagInfo GetTagInfoByTagName(string tagName)
        {
            return TagsInfo.Find(tagInfo => tagInfo.TagName == tagName);
        }

        public static string GetMdTagByTagName(string tagName)
        {
            return GetTagInfoByTagName(tagName).MdTag;
        }
    }
}