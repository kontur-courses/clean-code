using System;
using System.Collections.Generic;

namespace Markdown
{
    public class EscapingFinder : IEscapingFinder
    {
        public const char EscapeChar = '\\';
        private readonly HashSet<int> escapeCharPositions;
        private readonly HashSet<int> escapedMarkupCharsPositions;

        private readonly HashSet<char> markupChars;
        public readonly string Text;
        private TextInfo escapingInfoCache;

        public EscapingFinder(string text, HashSet<char> markupChars)
        {
            Text = text;
            this.markupChars = markupChars;
            escapeCharPositions = new HashSet<int>();
            escapedMarkupCharsPositions = new HashSet<int>();
        }

        public TextInfo Find()
        {
            if (escapingInfoCache != null)
                return escapingInfoCache;
            foreach (var markupChar in markupChars) FindEscaped(markupChar);
            escapingInfoCache = new TextInfo(Text, escapeCharPositions, escapedMarkupCharsPositions);
            return escapingInfoCache;
        }

        private void FindEscaped(char markupChar)
        {
            var pointer = 0;
            int position;
            while ((position = Text.IndexOf(markupChar, pointer)) != -1)
            {
                var count = GetEscapingCharCount(position);
                for (var i = 1; i < Math.Ceiling(count / 2.0) + 1; i++) escapeCharPositions.Add(position - i);
                if (count % 2 == 1)
                    escapedMarkupCharsPositions.Add(position);
                pointer = position + 1;
            }
        }

        private int GetEscapingCharCount(int markupCharPosition)
        {
            var count = 0;
            while (markupCharPosition != 0 && Text[markupCharPosition - 1] == EscapeChar)
            {
                count++;
                markupCharPosition--;
            }

            return count;
        }
    }
}