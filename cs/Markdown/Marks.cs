using System.Collections.Generic;
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
            var marks = new Dictionary<string, string>
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

            return marks[htmlTag];
        }

        public static string GetHtmlTagByMark(string mark)
        {
            var tags = new Dictionary<string, string>
            {
                {"_", "em"},
                {"__", "strong"},
                {"#", "h1"}
            };

            return tags[mark];
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
            var marksFirstLetters = new HashSet<char> {'_', '#'};


            return index + 1 < text.Length && marksFirstLetters.Contains(text[index]) &&
                   !char.IsWhiteSpace(text[index + 1]) ||
                   index + 1 == text.Length && marksFirstLetters.Contains(text[index]);
        }
    }
}