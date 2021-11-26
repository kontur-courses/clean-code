using System.Collections.Generic;

namespace MarkDown
{
    public static class Tokenizer
    {
        private static readonly List<Token> pseudoStaticTokens = new()
        {
            new BoldToken(0),
            new ItalicToken(0),
            new ListElementToken(0),
            new HeaderToken(0)
        };
        private static readonly List<char> protectedChars = new() { '_' };
        private static readonly char escape = '\\';

        public static Token GetToken(string text)
        {
            var state = new TokenizerState();
            var token = new Token(0, text.Length);
            state.currentToken = token;
            for (int i = 0; i < text.Length; i++)
            {
                if (state.wasIntersected)
                {
                    break;
                }
                if (TextHelper.IsCaseWhenShouldNotTokenize(text, state, i))
                {
                    state.MakeAllStatesFalse();
                }
                if (!protectedChars.Contains(text[i]))
                {
                    state.isEscaping = false;
                }
                HandleOpenedTokenSituation(text, state, i);
                HandleClosedTokensSituation(text, state, i);
            }
            TokenCleaner.CleanToken(token, text.Length);
            return token;
        }

        private static void HandleClosedTokensSituation(string text, TokenizerState state, int i)
        {
            if (TextHelper.CheckIfIthIsSpecificChar(text, i - 1, escape) || state.isEscaping)
            {
                state.isEscaping = true;
            }
            else
            {
                foreach (var staticToken in pseudoStaticTokens)
                {
                    if (staticToken.CanBeOpened(text, i) && !state.IsSpecificTokenOpened(staticToken))
                    {
                        state.OpenSpecificToken(i, text, staticToken.CreateNewTokenOfSameType(i));
                    }
                }
            }
        }

        private static void HandleOpenedTokenSituation(string text, TokenizerState state, int i)
        {
            var token = state.currentToken;
            if (i - token.Start < token.RawLengthOpen + token.RawLengthClose)
            {
                return;
            }

            if (TextHelper.IsIntersecionState(state, text, i))
            {
                state.wasIntersected = true;
                return;
            }

            if (state.IsSpecificTokenOpened(token)) 
            {
                if (token.CanBeClosed(text, i))
                {
                    state.CloseCurrentToken(i);
                }
            }
        }
    }
}
