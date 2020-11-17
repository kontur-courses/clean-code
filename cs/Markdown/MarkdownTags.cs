using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class MarkdownTags
    {
        private static readonly HashSet<string> marks = new HashSet<string> {"_", "__", "#"};

        private static readonly Dictionary<string, string> tagsByMark = new Dictionary<string, string>
        {
            {"_", "em"},
            {"__", "strong"},
            {"#", "h1"}
        };

        private static readonly Dictionary<string, string> pairs = new Dictionary<string, string>
        {
            {"_", "_"},
            {"__", "__"},
            {"#", "\n"}
        };

        public static string GetHtmlTagByMark(string mark)
        {
            return tagsByMark[mark];
        }

        public static string GetMarkPair(string mark)
        {
            return pairs[mark];
        }

        public static bool IsMark(string mark)
        {
            return marks.Contains(mark);
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