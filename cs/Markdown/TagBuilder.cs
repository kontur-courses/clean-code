using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TagBuilder
    {
        public static readonly Dictionary<string, string> markPairs = new Dictionary<string, string>
        {
            {"_", "_"},
            {"__", "__"},
            {"#", "\n"}
        };

        public static Tag BuildTag(string text, int startIndex, bool isScreened)
        {
            var mark = GetMark(text, startIndex);
            var tagName = GetHtmlTagByMark(mark);
            var closePosition = FindClosePosition(text, startIndex, mark);
            var content = GetContent(startIndex, closePosition, text);
            if (isScreened)
                content = RemoveScreeningSymbols(content);

            var tag = new Tag(tagName, isScreened)
            {
                OpenPosition = startIndex,
                ClosePosition = FindClosePosition(text, startIndex, mark),
                Content = content,
            };
            tag.Closed = IsTagClosed(tag, text);

            return tag;
        }

        private static int FindClosePosition(string text, int startIndex, string openedMark)
        {
            var i = startIndex + 1;
            var pairMark = markPairs[openedMark];
            var mark = "";
            while (i < text.Length && mark != pairMark)
            {
                mark = text.Substring(i, openedMark.Length);
                i++;
            }

            if (openedMark == "#" && i == text.Length)
                i++;

            return i - 1 + mark.Length - 1;
        }

        public static bool ExpectedToBeMark(char c)
        {
            var marksFirstLetters = new HashSet<char> {'_', '#'};
            return marksFirstLetters.Contains(c);
        }

        private static string GetContent(int startIndex, int endIndex, string text)
        {
            var contentBuilder = new StringBuilder();
            var mark = GetMark(text, startIndex);
            for (var i = startIndex + mark.Length; i <= endIndex - mark.Length; i++)
                contentBuilder.Append(text[i]);

            return contentBuilder.ToString();
        }

        private static string RemoveScreeningSymbols(string text)
        {
            var i = text.Length - 1;
            var slashCount = 0;
            while (text[text.Length-1] == text[i])
            {
                slashCount++;
                i--;
            }
            return text.Remove(text.Length - 1, 1);
        }

        private static string GetMark(string text, int start)
        {
            var i = start;
            var markBuilder = new StringBuilder();
            while (text[i] == text[start])
            {
                markBuilder.Append(text[i]);
                i++;
            }

            return markBuilder.ToString();
        }

        private static bool IsTagClosed(Tag tag, string text)
        {
            var mark = GetMarkByHtmlTag(tag.TagName);

            if (mark == "#")
                return true;

            return text.Substring(tag.OpenPosition, mark.Length) ==
                   text.Substring(tag.ClosePosition - mark.Length + 1, mark.Length)
                   && !tag.Content.Any(char.IsDigit);
        }

        public static string GetMarkByHtmlTag(string htmlTag)
        {
            var marks = new Dictionary<string, string>
            {
                {"em", "_"},
                {"strong", "__"},
                {"h1", "#"}
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
    }
}