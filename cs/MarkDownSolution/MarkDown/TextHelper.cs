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
            return CheckIfIthIsSpecificChar(text, i + 1, ground)
                && !CheckIfIthIsSpecificChar(text, i + 2, ' ')
                && !IsThreeGroundsInRow(text, i);
        }

        public static bool CanOpenItalicToken(string text, int i)
        {
            return !CheckIfIthIsSpecificChar(text, i + 1, ground)
                && !CheckIfIthIsSpecificChar(text, i + 1, ' ')
                && !CheckIfIthIsSpecificChar(text, i - 1, ground)
                && !IsThreeGroundsInRow(text, i);
        }

        public static bool CanCloseItalicToken(string text, int i)
        {
            return !(CheckIfIthIsSpecificChar(text, i - 1, ' ')
                  || CheckIfIthIsSpecificChar(text, i-1, ground)
                  || CheckIfIthIsSpecificChar(text, i + 1, ground));
        }

        public static bool CanCloseBoldToken(string text, int i)
        {
            return (!(CheckIfIthIsSpecificChar(text, i - 2, ' ')
                || CheckIfIthIsSpecificChar(text, i - 2, ground)
                || !CheckIfIthIsSpecificChar(text, i - 1, ground)))
                && CheckIfIthIsSpecificChar(text, i - 1, ground)
                && CheckIfIthIsSpecificChar(text, i, ground);
        }

        public static bool IsCaseWhenShouldNotTokenize(string text, TokenizerState state, int i)
        {
            return char.IsDigit(text[i]) || char.IsWhiteSpace(text[i]) && state.isSplittingWord || state.wasIntersected;
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

        private static bool IsThreeGroundsInRow(string text, int i)
        {
            var caseMinus2 = CheckIfIthIsSpecificChar(text, i - 2, ground);
            var caseMinus1 = CheckIfIthIsSpecificChar(text, i - 1, ground);
            var case0 = CheckIfIthIsSpecificChar(text, i + 0, ground);
            var case1 = CheckIfIthIsSpecificChar(text, i + 1, ground);
            var case2 = CheckIfIthIsSpecificChar(text, i + 2, ground);
            var situation1 = caseMinus2 && caseMinus1 && case0;
            var situation2 = caseMinus1 && case0 && case1;
            var situation3 = case0 && case1 && case2;
            return situation1 || situation2 || situation3;
        }
    }
}
