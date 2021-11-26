using System;

namespace MarkDown
{
    public static class TextHelper
    {
        private static readonly char ground = '_';

        public static bool CanCloseToken(string text, int i, Type tokenType)
        {
            if (tokenType == typeof(BoldToken))
            {
                var token = new BoldToken(0);
                return token.CanBeClosed(text, i);
            }
            else if (tokenType == typeof(ItalicToken))
            {
                var token = new ItalicToken(0);
                return token.CanBeClosed(text, i);
            }
            return false;
        }

        public static bool IsIntersecionState(TokenizerState state, string text, int i)
        {
            foreach (var key in state.statesDict.Keys)
            {
                if (state.statesDict[key])
                {
                    if (CanCloseToken(text, i, key))
                    {
                        if (state.currentToken.GetType() != key)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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

        public static bool IsThreeGroundsInRow(string text, int i)
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
