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

        public static void CloseItalicAndOpenBold(this TokenizerState state)
        {
            state.statesDict[CaseType.Bold] = true;
            state.statesDict[CaseType.Italic] = false;
        }

        public static bool IsSecondGroundInRow(this TokenizerState state, int i)
        {
            return i - state.start == 1;
        }

        public static bool IsSomeTokenOpened(this TokenizerState state)
        {
            return state.statesDict.ContainsValue(true);
        }

        public static void OpenItalicToken(this TokenizerState state, int i, string text)
        {
            state.isSplittingWord = CheckIfPreviousIsLetter(text, i);
            state.start = i;
            state.statesDict[CaseType.Italic] = true;
        }

        public static void CloseItalicToken(this TokenizerState state, int i)
        {
            state.currentToken.AddNestedToken(new ItalicToken(state.start, i - state.start));
            state.statesDict[CaseType.Italic] = false;
        }

        public static void CloseBoldToken(this TokenizerState state, int i)
        {
            state.currentToken.AddNestedToken(new BoldToken(state.start, i - state.start));
            state.statesDict[CaseType.Bold] = false;
        }

        public static void MakeAllStatesFalse(this TokenizerState state)
        {
            foreach (var pair in state.statesDict)
            {
                state.statesDict[pair.Key] = false;
            }
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
