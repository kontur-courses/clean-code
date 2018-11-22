using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringExtension
    {
        public static bool startWith(this string str, string word, int startPos)
        {
            if (str.Length - startPos < word.Length)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != str[i + startPos])
                {
                    return false;
                }
            }
            return true;
        }
        public static int indexOfCloseBracket(this string str, string word, int startPos)       // TODO Переименовать методы
        {
            int endIndex = -1;
            for (int i = startPos; i <= str.Length - word.Length; i++)
            {
                if (str.startWith(word, i))
                {
                    if (!(Char.IsWhiteSpace(str[i - 1]) || str[i - 1] == '\\'))
                        endIndex = i;
                }
            }
            return endIndex;
        }

        public static bool isInsideWord(this string text, int indexOfDigit)
        {
            return (indexOfDigit != 0 && Char.IsLetterOrDigit(text[indexOfDigit - 1])) &&
                   (indexOfDigit != text.Length - 1 && Char.IsLetterOrDigit(text[indexOfDigit + 1]));
        }
    }
}
