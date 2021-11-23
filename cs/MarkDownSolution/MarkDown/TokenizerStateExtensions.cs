using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public static class TokenizerStateExtensions
    {
        public static bool BoldTokenIsOpened(this TokenizerState state)
        {
            return state.statesDict[CaseType.Bold];
        }

        public static bool ItalicTokenIsOpened(this TokenizerState state)
        {
            return state.statesDict[CaseType.Italic];
        }

        public static bool IsSecondGroundInRow(this TokenizerState state, int i)
        {
            return i - state.currentToken.start == 1;
        }

        public static bool IsSomeTokenOpened(this TokenizerState state)
        {
            return state.statesDict.ContainsValue(true);
        }

        public static void OpenItalicToken(this TokenizerState state, int i, string text)
        {
            var token = new ItalicToken(i);
            state.currentToken.AddNestedToken(token);
            state.currentToken = token;
            state.isSplittingWord = TextHelper.CheckIfIthIsLetter(text, i - 1);
            state.statesDict[CaseType.Italic] = true;
        }

        public static void OpenBoldToken(this TokenizerState state, int i, string text)
        {
            var token = new BoldToken(i);
            state.currentToken.AddNestedToken(token);
            state.currentToken = token;
            state.isSplittingWord = TextHelper.CheckIfIthIsLetter(text, i - 1);
            state.statesDict[CaseType.Bold] = true;
        }

        public static void CloseItalicToken(this TokenizerState state, int i)
        {
            var token = state.currentToken;
            token.SetLength(1 + i - token.start);
            state.currentToken = token.fatherToken;
            state.statesDict[CaseType.Italic] = false;
        }

        public static void CloseBoldToken(this TokenizerState state, int i)
        {
            var token = state.currentToken;
            token.SetLength(1 + i - token.start);
            state.currentToken = token.fatherToken;
            state.statesDict[CaseType.Bold] = false;
        }

        public static void MakeAllStatesFalse(this TokenizerState state)
        {
            foreach (var pair in state.statesDict)
            {
                state.statesDict[pair.Key] = false;
            }
        }
    }
}
