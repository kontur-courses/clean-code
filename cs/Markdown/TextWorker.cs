using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TextWorker
    {
        public static IEnumerable<string> SplitOnParagraphs(string text)
        {
            return text.Split(Environment.NewLine);
        }

        public static string RemoveShieldsBeforeKeyChars(string text, IEnumerable<char> keyChars, char shield = '/')
        {
            var shielded = new HashSet<char>(keyChars) {shield};
            for (var i = 0; i < text.Length - 1; i++)
                if (text[i] == shield && shielded.Contains(text[i + 1]))
                    text = text.Remove(i, 1);
            return text;
        }
    }
}