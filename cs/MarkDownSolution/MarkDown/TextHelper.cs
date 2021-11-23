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
        
        public static bool CanOpenBoldToken(string text, int i)
        {
            return CheckIfIthIsSpecificChar(text, i + 1, '_')
                && !CheckIfIthIsSpecificChar(text, i + 2, ' ');
        }

        public static bool CanOpenItalicToken(string text, int i)
        {
            return !CheckIfIthIsSpecificChar(text, i + 1, '_')
                && !CheckIfIthIsSpecificChar(text, i + 1, ' ')
                && !CheckIfIthIsSpecificChar(text, i - 1, '_');
        }

        public static bool CanCloseItalicToken(string text, int i)
        {
            return !(CheckIfIthIsSpecificChar(text, i - 1, ' ') ||
                CheckIfIthIsSpecificChar(text, i-1, ground) ||
                     CheckIfIthIsSpecificChar(text, i + 1, ground));
        }

        public static bool CanCloseBoldToken(string text, int i)
        {
            return !(CheckIfIthIsSpecificChar(text, i - 2, ' ')
                || CheckIfIthIsSpecificChar(text, i - 2, ground)
                || !CheckIfIthIsSpecificChar(text, i - 1, ground));
        }

        public static bool IsCaseWhenShouldNotTokenize(string text, TokenizerState state, int i)
        {
            return char.IsDigit(text[i]) || char.IsWhiteSpace(text[i]) && state.isSplittingWord;
        }

        public static bool CheckIfIthIsSpecificChar(string text, int i, char ch)
        {
            if (i >= text.Length || i < 0)
            {
                return false;
            }
            return text[i] == ch;
        }

        public static bool CheckIfIthIsLetter(string text, int i)
        {
            if (i >= text.Length || i < 0)
            {
                return false;
            }
            return char.IsLetter(text[i]);
        }
    }
}
