using System.Collections.Generic;

namespace Markdown
{
    public class TextInfo
    {
        public readonly HashSet<int> EscapedMarkupCharsPositions;
        public readonly HashSet<int> EscapeCharPositions;
        public readonly string Text;

        public TextInfo(string text, HashSet<int> escapeCharPositions, HashSet<int> escapedMarkupCharsPositions)
        {
            Text = text;
            EscapeCharPositions = escapeCharPositions;
            EscapedMarkupCharsPositions = escapedMarkupCharsPositions;
        }
    }
}