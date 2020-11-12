using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagBuilder
    {
        public static readonly Dictionary<string, string> tags = new Dictionary<string, string>
        {
            {"_", "em"},
            {"__", "strong"},
            {"#", "h1"}
        };
        
        public static readonly Dictionary<string, string> marks = new Dictionary<string, string>
        {
            {"em", "_"},
            {"strong", "__"},
            {"h1", "#"}
        };
        
        public static readonly Dictionary<string, string> markPairs = new Dictionary<string, string>
        {
            {"_", "_"},
            {"__", "__"},
            {"#", "\n"}
        };
        
        public static Tag BuildTag(string text, int startIndex, bool isScreened)
        {
            var tagName = tags[$"{text[startIndex]}"];;
            if (text[startIndex] == '_' && startIndex < text.Length - 1 && text[startIndex + 1] == '_')
                tagName = "strong";

            var mark = marks[tagName];
            var closePosition = FindClosePosition(text, startIndex, mark);
            
            return new Tag(tagName, startIndex, closePosition, isScreened, text);;
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

            return i - 1 + mark.Length - 1;
        }

        public static bool ExpectedToBeMark(char c)
        {
            return tags.Keys.Select(mark => mark[0]).Contains(c);
        }
    }
}