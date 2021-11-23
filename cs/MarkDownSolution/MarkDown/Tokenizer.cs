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
            if (state.IsSomeTokenOpened())
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
            if (TextHelper.CheckIfPreviousIsSpecificChar(text, i, escape) || state.isEscaping)
            {
                state.isEscaping = true;
            }
            else if (!TextHelper.CheckIfNextsIsSpecificChar(text, i, ground))
            {
                state.OpenItalicToken(i, text);
            }
            else
            {
                state.OpenBoldToken(i, text);
            }
        }

        private static void HandleOpenedTokenSituation(string text, int i, TokenizerState state)
        {
            if (state.IsSecondGroundInRow(i))
            {
                state.CloseItalicAndOpenBold();
            }
            else if (state.ItalicTokenIsOpened())
            {
                if (TextHelper.CanCloseItalicToken(text, i))
                {
                    state.CloseItalicToken(i);
                }
            }
            else if (state.BoldTokenIsOpened())
            {
                if (TextHelper.CanCloseBoldToken(text, i))
                {
                    state.CloseBoldToken(i);
                }
            }
        }
    }
}
