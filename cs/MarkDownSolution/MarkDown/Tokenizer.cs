using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarkDown
{
    public static class Tokenizer
    {
        private static char header = '#';
        private static char ground = '_';
        private static char escape = '\\';

        public static Token GetToken(string text)
        {
            var state = new TokenizerState();
            var token = new Token(0, text.Length);
            state.currentToken = token;
            if (text[0] == header)
            {
                state.currentToken.nestedTokens.Add(new HeaderToken(0, text.Length));
            }
            for (int i = 0; i < text.Length; i++)
            {
                if (isCaseWhenShouldNotTokenize(text, state.isSplittingWord, i))
                {
                    MakeAllStatesFalse(state);
                }
                if (text[i] == ground)
                {
                    HandleGroundSituation(text, state, i);
                }
                else
                {
                    state.isEscaping = false;
                }
            }
            return token;
        }

        private static void HandleGroundSituation(string text, TokenizerState state, int i)
        {
            if (SomeTokenIsOpened(state))
            {
                HandleOpenedTokenSituation(text, i, state);
            }
            else
            {
                HandleClosedTokensSituation(text, state, i);
            }
        }

        private static void HandleClosedTokensSituation(string text, TokenizerState state, int i)
        {
            if (CheckIfPreviousIsSpecificChar(text, i, escape) || state.isEscaping)
            {
                state.isEscaping = true;
            }
            else
            {
                OpenItalicToken(text, state, i);
            }
        }

        private static void OpenItalicToken(string text, TokenizerState state, int i)
        {
            state.isSplittingWord = CheckIfPreviousIsLetter(text, i);
            state.start = i;
            state.statesDict[CaseType.Italic] = true;
        }

        private static void HandleOpenedTokenSituation(string text, int i, TokenizerState state)
        {
            if (IsSecondGroundInRow(state, i))
            {
                CloseItalicAndOpenBold(state);
            }
            else if (ItalicTokenIsOpened(state))
            {
                if (CanCloseItalicToken(text, i))
                {
                    CloseItalicToken(state, i);
                }
            }
            else if (BoldTokenIsOpened(state))
            {
                if (CanCloseBoldToken(text, i))
                {
                    CloseBoldToken(state, i);
                }
            }
        }

        private static bool BoldTokenIsOpened(TokenizerState state)
        {
            return state.statesDict[CaseType.Bold];
        }

        private static bool ItalicTokenIsOpened(TokenizerState state)
        {
            return state.statesDict[CaseType.Italic];
        }

        private static void CloseItalicAndOpenBold(TokenizerState state)
        {
            state.statesDict[CaseType.Bold] = true;
            state.statesDict[CaseType.Italic] = false;
        }

        private static bool IsSecondGroundInRow(TokenizerState state, int i)
        {
            return i - state.start == 1;
        }

        private static bool SomeTokenIsOpened(TokenizerState state)
        {
            return state.statesDict.ContainsValue(true);
        }

        private static bool CanCloseItalicToken(string text, int i)
        {
            return !CheckIfPreviousIsSpecificChar(text, i, ' ');
        }

        private static bool CanCloseBoldToken(string text, int i)
        {
            return !(CheckIfPreviousIsSpecificChar(text, i - 1, ' ') 
                || CheckIfPreviousIsSpecificChar(text, i - 1, ground) 
                || !CheckIfPreviousIsSpecificChar(text, i, ground));
        }

        private static void CloseItalicToken(TokenizerState state, int i)
        {
            state.currentToken.nestedTokens.Add(new ItalicToken(state.start, i - state.start));
            state.statesDict[CaseType.Italic] = false;
        }

        private static void CloseBoldToken(TokenizerState state, int i)
        {
            state.currentToken.nestedTokens.Add(new BoldToken(state.start, i - state.start));
            state.statesDict[CaseType.Bold] = false;
        }

        private static bool isCaseWhenShouldNotTokenize(string text, bool isSplittingWord, int i)
        {
            return char.IsDigit(text[i]) || char.IsWhiteSpace(text[i]) && isSplittingWord;
        }

        private static void MakeAllStatesFalse(TokenizerState state)
        {
            foreach (var pair in state.statesDict)
            {
                state.statesDict[pair.Key] = false;
            }
        }

        private static bool CheckIfPreviousIsSpecificChar(string text, int i, char ch)
        {
            if (i == 0)
            {
                return false;
            }
            return text[i - 1] == ch;
        }

        private static bool CheckIfPreviousIsLetter(string text, int i)
        {
            if (i == 0)
            {
                return false;
            }
            return char.IsLetter(text[i - 1]);
        }
    }
}
