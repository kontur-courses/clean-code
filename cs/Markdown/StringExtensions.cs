using System;

namespace Markdown
{
    public static class StringExtensions
    {
        public static int GetIndexOfCloseTag(this string str, string word, int startPos)       
        {   /*
            Найти первое вхождение
                Если не нашел - вернуть -1
            Идти вправо пока не закончится суффикс


            */


            int endIndex = str.IndexOf(word, startPos);
            /*for (int i = startPos; i <= str.Length - word.Length; i++)
            {
                if (str.IndexOf(word, i) != -1)
                {
                    if (!(Char.IsWhiteSpace(str[i - 1]) || str[i - 1] == '\\'))
                        endIndex = i;
                }
            }*/
            return endIndex;
        }

        public static bool IsInsideWord(this string text, int index, int len)
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

            for (int i = index + 1 + len; i < text.Length; i++)
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
