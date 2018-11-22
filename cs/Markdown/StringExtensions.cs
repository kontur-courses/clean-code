using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool startWith(this string str, string word, int startPos = 0)
        {
            if (str.Length - startPos < word.Length)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != str[i + startPos])
                    return false;
            }
            return true;
        }
        public static int indexOfCloseTag(this string str, string word, int startPos)       // TODO Переименовать методы
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

        public static bool isInsideWord(this string text, int index, int wordLen)
        {
            return (index != 0 && Char.IsLetterOrDigit(text[index - 1])) &&
                   (index < text.Length - wordLen - 1 && Char.IsLetterOrDigit(text[index + wordLen]));
        }
    }
}
