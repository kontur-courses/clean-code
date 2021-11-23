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
                var newToken = new HeaderToken(0, text.Length);
                state.currentToken.AddNestedToken(newToken);
                state.currentToken = newToken;
            }
            for (int i = 0; i < text.Length; i++)
            {
                if (TextHelper.IsCaseWhenShouldNotTokenize(text, state, i))
                {
                    state.MakeAllStatesFalse();
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
            TokenCleaner.CleanToken(token);
            return token;
        }

        private static void HandleGroundSituation(string text, TokenizerState state, int i)
        {
            HandleOpenedTokenSituation(text, i, state);
            HandleClosedTokensSituation(text, state, i);
        }

        private static void HandleClosedTokensSituation(string text, TokenizerState state, int i)
        {
            if (TextHelper.CheckIfIthIsSpecificChar(text, i - 1, escape) || state.isEscaping)
            {
                state.isEscaping = true;
            }
            else if (TextHelper.CanOpenItalicToken(text, i) && !state.ItalicTokenIsOpened())
            {
                state.OpenItalicToken(i, text);
            }
            else if (TextHelper.CanOpenBoldToken(text, i) && !state.BoldTokenIsOpened())
            {
                state.OpenBoldToken(i, text);
            }
        }

        private static void HandleOpenedTokenSituation(string text, int i, TokenizerState state)
        {
            if (state.ItalicTokenIsOpened())
            {
                if (TextHelper.CanCloseItalicToken(text, i) && i - state.currentToken.start >= 2)
                {
                    state.CloseItalicToken(i);
                    return;
                }
            }
            if (state.BoldTokenIsOpened())
            {
                if (TextHelper.CanCloseBoldToken(text, i) && i - state.currentToken.start >= 4)
                {
                    state.CloseBoldToken(i);
                }
            }
        }
    }
}
