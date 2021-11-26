using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MarkDown
{
    public static class Tokenizer
    {
        private static readonly List<Token> pseudoStaticTokens = new()
        {
            new BoldToken(0),
            new ItalicToken(0)
        };
        private static readonly char header = '#';
        private static readonly char ground = '_';
        private static readonly char escape = '\\';

        public static Token GetToken(string text)
        {
            var state = new TokenizerState();
            var token = new Token(0, text.Length);
            state.currentToken = token;
            if (text[0] == header)
            {
                var newToken = new HeaderToken(0, text.Length);
                state.currentToken.AddNestedToken(newToken);
                state.currentToken = newToken;
            }
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
                if (text[i] == ground)
                {
                    HandleOpenedTokenSituation(text, state, i);
                    HandleClosedTokensSituation(text, state, i);
                }
                else
                {
                    state.isEscaping = false;
                }
            }
            TokenCleaner.CleanToken(token);
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
