using System.Text;

namespace Markdown
{
    public class TagSettings
    {
        public static bool IsHalfOfWord(StringBuilder markdownText, int i)
        {
            return i > 0 && markdownText[i - 1] != ' ' &&
                   i + 1 < markdownText.Length && markdownText[i + 1] != ' ';
        }

        public static bool IsCorrectClosingSymbol(StringBuilder markdownText, int i, char symbol) =>
            i - 1 >= 0 && markdownText[i - 1] != ' ' && markdownText[i - 1] != symbol;

        public static bool IsCorrectOpenSymbol(StringBuilder markdownText, int i) =>
            i + 1 < markdownText.Length && markdownText[i + 1] != ' ';
    }
}