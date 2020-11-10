using System.Collections.Generic;

namespace Markdown
{
    public enum TagType
    {
        Italic,
        Bold,
        Header
    }

    public static class TagTypeExtensions
    {
        private static readonly Dictionary<TagType, string> markdownTags = new Dictionary<TagType, string>
        {
            [TagType.Italic] = "_",
            [TagType.Bold] = "__",
            [TagType.Header] = "#"
        };

        private static readonly Dictionary<TagType, string> openHtmlTags = new Dictionary<TagType, string>
        {
            [TagType.Italic] = "<em>",
            [TagType.Bold] = "<strong>",
            [TagType.Header] = "<h1>"
        };

        private static readonly Dictionary<TagType, string> closeHtmlTags = new Dictionary<TagType, string>
        {
            [TagType.Italic] = openHtmlTags[TagType.Italic].Insert(1, "/"),
            [TagType.Bold] = openHtmlTags[TagType.Bold].Insert(1, "/"),
            [TagType.Header] = openHtmlTags[TagType.Header].Insert(1, "/")
        };

        public static string GetMarkdownTag(this TagType tagType)
        {
            return markdownTags[tagType];
        }

        public static int GetMarkdownTagLength(this TagType tagType)
        {
            return markdownTags[tagType].Length;
        }

        public static string GetOpenHtmlTag(this TagType tagType)
        {
            return openHtmlTags[tagType];
        }

        public static string GetCloseHtmlTag(this TagType tagType)
        {
            return closeHtmlTags[tagType];
        }
    }
}