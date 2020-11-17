using System.Collections.Generic;

namespace Markdown
{
    public static class TextParserExtensions
    {
        public static bool IsAfterBackslash(this TextParser parser, string text, int index)
        {
            var escapingBackslashes = parser.FindEscapingBackslashes(text);

            return index != 0 && escapingBackslashes.Contains(index - 1);
        }

        public static List<int> FindEscapingBackslashes(this TextParser parser, string text)
        {
            var positions = new List<int>();

            for (var i = 0; i < text.Length; ++i)
                if (IsEscapingBackslash(text, i) && !positions.Contains(i - 1))
                    positions.Add(i);

            return positions;
        }

        private static bool IsEscapingBackslash(string text, int index)
        {
            return text[index] == '\\'
                   && !char.IsLetter(text[index + 1]);
        }
    }
}