using System.Text;

namespace Markdown.Parser.Inline
{
    public static class Escaper
    {
        public static int GetNotEscapedClosingCharIndex(string text, int startIndex, char targetChar)
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

        public static string HandleEscapeForWord(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var buffer = new StringBuilder(text.Length);
            var i = 0;

            while (i < text.Length)
            {
                if (text[i] == '\\')
                {
                    i += HandleEscape(text, i, buffer);
                }
                else
                {
                    buffer.Append(text[i]);
                    i++;
                }
            }

            return buffer.ToString();
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