using System.Collections.Generic;

namespace Markdown
{
    public class TextWorker
    {
        public static IEnumerable<string> SplitOnParagraphs(string text)
        {
            return text.Split("\n\r");
        }

        public static string RemoveShieldsBeforeKeyChars(string text, IEnumerable<char> keyChars, char shield = '/')
        {
            var shilded = new HashSet<char>(keyChars) {shield};
            for (var i = 0; i < text.Length - 1; i++)
                if (text[i] == shield && shilded.Contains(text[i + 1]))
                    text = text.Remove(i, 1);
            return text;
        }
    }
}