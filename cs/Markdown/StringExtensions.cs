using System;

namespace Markdown
{
    public static class StringExtensions
    {
        public static int getIndexOfCloseTag(this string str, string word, int startPos)       
        {
            int endIndex = -1;
            for (int i = startPos; i <= str.Length - word.Length; i++)
            {
                if (str.IndexOf(word, i) != -1)
                {
                    if (!(Char.IsWhiteSpace(str[i - 1]) || str[i - 1] == '\\'))
                        endIndex = i;
                }
            }
            return endIndex;
        }

        public static bool isInsideWord(this string text, int index)
        {
            bool leftSide = false, rightSide = false;

            for (int i = index-1; i >= 0; i--)
            {
                if (Char.IsLetterOrDigit(text[i]))
                {
                    leftSide = true;
                    break;
                }

                if (Char.IsWhiteSpace(text[i]))         // TODO Рассмотреть какие еще символы продходят кроме пробелов
                    break;
            }

            for (int i = index + 1; i < text.Length; i++)
            {
                if (Char.IsLetterOrDigit(text[i]))
                {
                    rightSide = true;
                    break;
                }
                if (Char.IsWhiteSpace(text[i]))       
                    break;
            }

            return leftSide && rightSide;
        }
    }
}
