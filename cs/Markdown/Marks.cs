using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Marks
    {
        private static readonly HashSet<string> marks = new HashSet<string> {"_", "__", "#"};

        private static readonly HashSet<string> htmlTags = new HashSet<string>
            {"<h1>", "</h1>", "<em>", "</em>", "<strong>", "</strong>"};

        public static string GetMarkByHtmlTag(string htmlTag)
        {
            var marksByTag = new Dictionary<string, string>
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

            return marksByTag[htmlTag];
        }

        public static string GetHtmlTagByMark(string mark)
        {
            var tagsByMark = new Dictionary<string, string>
            {
                {"_", "em"},
                {"__", "strong"},
                {"#", "h1"}
            };

            return tagsByMark[mark];
        }

        public static string GetMarkPair(string mark)
        {
            var pairs = new Dictionary<string, string>
            {
                {"_", "_"},
                {"__", "__"},
                {"#", "\n"}
            };

            return pairs[mark];
        }

        public static bool IsClosedHtmlTag(string tag)
        {
            var closedHtmlTags = new HashSet<string> {"</em>", "</strong>", "</h1>", "</a>"};
            return closedHtmlTags.Contains(tag);
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

        public static bool IsMark(string mark)
        {
            return marks.Contains(mark);
        }

        public static bool IsHtmlTag(string tag)
        {
            return htmlTags.Contains(tag);
        }

        public static string GetMarkFromText(int start, string text)
        {
            var i = start;
            var markBuilder = new StringBuilder();
            while (i < text.Length && text[i] == text[start])
            {
                markBuilder.Append(text[i]);
                i++;
            }

            return markBuilder.ToString();
        }

        public static bool ExpectedToBeMark(int index, string text)
        {
            var marksFirstLetters = marks.Select(mark => mark[0]).ToHashSet();

            return index + 1 < text.Length && marksFirstLetters.Contains(text[index]) &&
                   !char.IsWhiteSpace(text[index + 1]) ||
                   index + 1 == text.Length && marksFirstLetters.Contains(text[index]);
        }
    }
}