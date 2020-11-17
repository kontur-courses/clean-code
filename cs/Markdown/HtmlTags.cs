using System.Collections.Generic;

namespace Markdown
{
    public static class HtmlTags
    {
        private static readonly HashSet<string> openTags = new HashSet<string> {"<h1>", "<em>", "<strong>"};
        private static readonly HashSet<string> closeTags = new HashSet<string> {"</h1>", "</em>", "</strong>"};

        private static readonly Dictionary<string, string> marksByTag = new Dictionary<string, string>
        {
            {"em", "_"},
            {"strong", "__"},
            {"h1", "#"},

            {"<em>", "_"},
            {"<strong>", "__"},
            {"<h1>", "#"},

            {"</em>", "_"},
            {"</strong>", "__"},
            {"</h1>", "#"}
        };

        public static string GetMarkByHtmlTag(string mark)
        {
            return marksByTag[mark];
        }

        public static bool IsClosedHtmlTag(string tag)
        {
            return closeTags.Contains(tag);
        }

        public static TagType GetTagType(string tagName)
        {
            switch (tagName)
            {
                case "em":
                case "<em>":
                case "</em>":
                    return TagType.Italic;

                case "strong":
                case "<strong>":
                case "</strong>":
                    return TagType.Bold;

                case "h1":
                case "<h1>":
                case "</h1>":
                    return TagType.Heading;

                default:
                    return TagType.Incorrect;
            }
        }

        public static bool IsHtmlTag(string tag)
        {
            return openTags.Contains(tag) || closeTags.Contains(tag);
        }
    }
}