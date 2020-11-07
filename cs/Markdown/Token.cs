using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly int StartIndex;
        public readonly TagOption TagOption;

        public Token(int startIndex, int length, TagOption tagOption)
        {
            throw new NotImplementedException();
        }

        public static Token[] ParseStringToTokens(string sourceText)
        {
            throw new NotImplementedException();
        }

        public string ApplyTokensToString(string sourceText, Token[] tokens)
        {
            throw new NotImplementedException();
        }

        private string AddItalicTagToString(string sourceText, Token token)
        {
            throw new NotImplementedException();
        }

        private string AddBoldTagToString(string sourceText, Token token)
        {
            throw new NotImplementedException();
        }

        private string AddHeaderTagToString(string sourceText, Token token)
        {
            throw new NotImplementedException();
        }

        private void IncreaseTokensLength(Token[] tokens, Token lastAppliedToken)
        {
            throw new NotImplementedException();
        }
    }
}