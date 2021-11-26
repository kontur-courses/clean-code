using System;
using System.Linq;

namespace MarkDown
{
    public static class TextPreparer
    {
        private static readonly string paragraphSymbol = Environment.NewLine;
        private static string PrepareParagraph(string text)
        {
            return text;
        }
        public static string[] PrepareText(string text)
        {
            var splittedText = text.Split(paragraphSymbol);
            return splittedText.Select(x => PrepareParagraph(x)).ToArray();
        }
    }
}
