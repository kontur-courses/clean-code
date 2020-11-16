using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Marks
    {
        private static readonly HashSet<string> marks = new HashSet<string>{"_", "__", "#"};
        
        public static string GetMarkByHtmlTag(string htmlTag)
        {
            var marks = new Dictionary<string, string>
            {
                {"em", "_"},
                {"strong", "__"},
                {"h1", "#"},
                {"", ""}
            };

            return marks[htmlTag];
        }

        public static string GetHtmlTagByMark(string mark)
        {
            var tags = new Dictionary<string, string>
            {
                {"_", "em"},
                {"__", "strong"},
                {"#", "h1"},
                {"", ""}
            };

            return tags[mark];
        }

        public static string GetMarkPair(string mark)
        {
            var pairs = new Dictionary<string, string>
            {
                {"_", "_"},
                {"__", "__"},
                {"#", "\n"},
                {"", ""}
            };

            return pairs[mark];
        }

        public static bool IsMarkExist(string mark)
        {
            return marks.Contains(mark);
        }
        
        public static string GetMarkFromText(string text, int start)
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
        
        public static bool ExpectedToBeMark(char c)
        {
            var marksFirstLetters = new HashSet<char> {'_', '#'};
            return marksFirstLetters.Contains(c);
        }
        
    }
}