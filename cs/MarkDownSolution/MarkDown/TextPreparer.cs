using System;
using System.Linq;
using System.Text;

namespace MarkDown
{
    public static class TextPreparer
    {
        private static string paragraphSymbol = Environment.NewLine;
        public static string PrepareParagraph(string text)
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
