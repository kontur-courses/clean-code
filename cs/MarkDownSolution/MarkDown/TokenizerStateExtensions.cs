namespace MarkDown
{
    public static class TokenizerStateExtensions
    {
        public static bool IsSpecificTokenOpened(this TokenizerState state, Token tokenOfThisType)
        {
            var gettedType = tokenOfThisType.GetType();
            if (state.statesDict.ContainsKey(gettedType))
            {
                return state.statesDict[tokenOfThisType.GetType()];
            }
            return false;
        }

        public static bool IsSecondGroundInRow(this TokenizerState state, int i)
        {
            return i - state.currentToken.Start == 1;
        }

        public static bool IsSomeTokenOpened(this TokenizerState state)
        {
            return state.statesDict.ContainsValue(true);
        }

        public static void OpenSpecificToken(this TokenizerState state, int i, string text, Token token)
        {
            state.currentToken.AddNestedToken(token);
            state.currentToken = token;
            state.isSplittingWord = TextHelper.CheckIfIthIsLetter(text, i - 1);
            state.statesDict[token.GetType()] = true;
        }

        public static void CloseCurrentToken(this TokenizerState state, int i)
        {
            var token = state.currentToken;
            token.SetLength(1 + i - token.Start);
            state.currentToken = token.FatherToken;
            state.statesDict[token.GetType()] = false;
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
