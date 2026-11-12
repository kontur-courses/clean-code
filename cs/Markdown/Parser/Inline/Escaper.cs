using System.Text;

namespace Markdown.Parser.Inline
{
    public static class Escaper
    {
        public static int FindClosingChar(string text, int startIndex, char targetChar)
        {
            for (var i = startIndex; i < text.Length; i++)
            {
                if (text[i] == '\\')
                {
                    i++;
                    continue;
                }

                if (text[i] == targetChar)
                {
                    return i;
                }
            }
            return -1;
        }

        public static string Unescape(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text
                .Replace("\\)", ")")
                .Replace("\\(", "(")
                .Replace("\\\\", "\\");
        }

        public static int HandleEscape(string text, int i, StringBuilder buffer)
        {
            if (i + 2 < text.Length && text[i + 1] == '_' && text[i + 2] == '_')
            {
                buffer.Append("__");
                return 3;
            }

            if (i + 1 < text.Length)
            {
                buffer.Append(text[i + 1]);
                return 2;
            }

            buffer.Append('\\');
            return 1;
        }
    }
}