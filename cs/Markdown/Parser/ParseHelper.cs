using System.Runtime.CompilerServices;
using static System.Char;

namespace Markdown.Parser
{
    public class ParseHelper
    {
        public static bool IsItalicStart(string data, int i)
        {
            if (data.Length <= i + 1)
                return false;

            if (i > 0 && IsUnderline(data[i - 1]))
            {
                return false;
            }

            return IsUnderline(data[i])
                   && !IsSpace(data[i + 1])
                   && !IsDigit(data[i + 1])
                   && !IsUnderline(data[i + 1]);
        }

        public static bool IsItalicEnd(string data, int i)
        {
            if (i < 1 || data.Length <= i)
                return false;

            return IsUnderline(data[i])
                   && !IsSpace(data[i - 1])
                   && !IsDigit(data[i - 1])
                   && !IsBoldEnd(data, i)
                   && !IsBoldEnd(data, i + 1)
                   && !IsBoldStart(data, i - 1);
        }

        public static bool IsBoldStart(string data, int i)
        {
            if (data.Length <= i + 2)
                return false;

            return IsUnderline(data[i])
                   && IsUnderline(data[i + 1])
                   && !IsDigit(data[i + 2])
                   && !IsSpace(data[i + 2]);
        }

        public static bool IsBoldEnd(string data, int i)
        {
            if (i < 2 || data.Length <= i)
                return false;

            if (i + 1 < data.Length && IsUnderline(data[i + 1]))
                return false;

            return IsUnderline(data[i])
                   && IsUnderline(data[i - 1])
                   && !IsDigit(data[i - 2])
                   && !IsSpace(data[i - 2])
                   && !IsUnderline(data[i - 2]);
        }

        public static string ReadToNextUnderline(string data, int start, out int underlineIndex)
        {
            for (underlineIndex = start + 1; underlineIndex < data.Length; underlineIndex++)
            {
                if (!IsUnderline(data[underlineIndex])) continue;
                return data.Substring(start, underlineIndex - start);
            }

            return data.Substring(start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSpace(char i)
        {
            return i == ' ';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnderline(char i)
        {
            return i == '_';
        }
    }
}