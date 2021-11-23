using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public static class TextHelper
    {
        private static char ground = '_';
        public static bool CanCloseItalicToken(string text, int i)
        {
            return !(CheckIfPreviousIsSpecificChar(text, i, ' ') ||
                CheckIfPreviousIsSpecificChar(text, i, ground) ||
                     CheckIfNextsIsSpecificChar(text, i, ground));
        }

        public static bool CanCloseBoldToken(string text, int i)
        {
            return !(CheckIfPreviousIsSpecificChar(text, i - 1, ' ')
                || CheckIfPreviousIsSpecificChar(text, i - 1, ground)
                || !CheckIfPreviousIsSpecificChar(text, i, ground));
        }

        public static bool IsCaseWhenShouldNotTokenize(string text, TokenizerState state, int i)
        {
            return char.IsDigit(text[i]) || char.IsWhiteSpace(text[i]) && state.isSplittingWord;
        }

        public static bool CheckIfPreviousIsSpecificChar(string text, int i, char ch)
        {
            if (i == 0)
            {
                return false;
            }
            return text[i - 1] == ch;
        }

        public static bool CheckIfNextsIsSpecificChar(string text, int i, char ch)
        {
            if (i == text.Length - 1)
            {
                return false;
            }
            return text[i + 1] == ch;
        }

        public static bool CheckIfPreviousIsLetter(string text, int i)
        {
            if (i == 0)
            {
                return false;
            }
            return char.IsLetter(text[i - 1]);
        }
    }
}
